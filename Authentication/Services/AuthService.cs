using ErrorOr;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Un2TrekApp.Domain;
using Un2TrekApp.Storage;

namespace Un2TrekApp.Authentication;

internal abstract class TokenServiceBase
{
    protected readonly ILocalStorage _localStorage;
    private readonly HttpClient _httpClient;

    protected TokenServiceBase(ILocalStorage localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }
    
    public async Task<string> GetTokenForCall()
    {
        var currentToken = await this.GetTokenFromLocalStorage();
        if (IsAboutExpire(currentToken))
        {
            var renewedToken = await RenewToken();
            if (renewedToken != null)
            {
                await SetTokenToLocalStorage(renewedToken);
                return renewedToken.JwtSecurityToken;
            }
        }
        return currentToken.JwtSecurityToken;

    }

    public JwtSecurityToken DecodedTokenValue(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var decodedValue = handler.ReadJwtToken(token);

        return decodedValue;
    }

    public AuthenticationResponse AutenticationResponseFromToken(string token)
    {
        AuthenticationResponse result = new();
        var decodedValue = DecodedTokenValue(token);
        result.JwtSecurityToken = token;
        result.ExpirationDate = decodedValue.ValidTo;

        return result;
    }

    private bool IsAboutExpire(AuthenticationData token)
    {
        var dateToExpire = token.ExpirationDate;
        var currentDatetime = DateTime.UtcNow;
        var diffOfDates = dateToExpire - currentDatetime;

        return (diffOfDates.Minutes < 10);
    }

    private async Task SetTokenToLocalStorage(AuthenticationResponse token)
    {
        string serializedUserInfo = await _localStorage.GetAsync(App.StorageUserInfoKey);
        var userInfo = JsonSerializer.Deserialize<User>(serializedUserInfo);

        userInfo.AuthData = new AuthenticationData { ExpirationDate = token.ExpirationDate, JwtSecurityToken = token.JwtSecurityToken };
        await _localStorage.SetAsync(App.StorageUserInfoKey, JsonSerializer.Serialize(userInfo));
    }

    private async Task<AuthenticationData> GetTokenFromLocalStorage()
    {
        string serializedUserInfo = await _localStorage.GetAsync(App.StorageUserInfoKey);
        var userInfo = JsonSerializer.Deserialize<User>(serializedUserInfo);
        if (userInfo != null)
        {
            return userInfo.AuthData;
        }

        return null;
    }

    
    private async Task<AuthenticationResponse> RenewToken()
    {        
        var token = await this.GetTokenFromLocalStorage();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token.JwtSecurityToken);

        StringBuilder request = new StringBuilder("Authentication/renew");

        using (HttpResponseMessage response = await _httpClient.GetAsync(request.ToString()))
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return AutenticationResponseFromToken(json);
            }
        }

        return null;
    }
}

internal class AuthService : TokenServiceBase, IAuthService
{
    private readonly HttpClient _httpClient;
    public AuthService(HttpClient httpClient, ILocalStorage localStorage) : base(localStorage, httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ErrorOr<LoginResponse>> AuthenticateAsync(LoginRequest userCredentials)
    {   
        var response = await _httpClient.PostAsJsonAsync("login", new { email = userCredentials.Email, password = userCredentials.Password });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
            return result ?? new LoginResponse();
        }

        var problemDetailJson = await response.Content.ReadAsStringAsync();
        var problemDetail = JsonSerializer.Deserialize<ProblemDetailResponse>(problemDetailJson);

        return Error.Unexpected(description: problemDetail?.detail ?? "Unknown error");
    }

    public async Task<ErrorOr<Success>> RegisterAsync(RegistrationRequest registrationRequest)
    {
        var requestContent = new StringContent(JsonSerializer.Serialize(registrationRequest), Encoding.UTF8, "application/json");
        var url = "Authentication/register";
        var response = await _httpClient.PostAsync(url, requestContent);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success;
        }
        
        return Error.Unexpected(description: "No se ha podido registrar el usuario");

    }
    public User SetUserInfoDataFromAuthResponse(string token)
    {
        var result = new User();
        var handler = new JwtSecurityTokenHandler();
        var decodedValue = DecodedTokenValue(token);

        foreach (var keyValuePair in decodedValue.Payload)
        {
            if (keyValuePair.Key.Contains("email"))
            {
                result.Email = keyValuePair.Value.ToString();
                result.UserName = keyValuePair.Value.ToString();
            }
            if (keyValuePair.Key.Contains("nameid"))
            {
                result.UserId = keyValuePair.Value.ToString();
            }
            if (keyValuePair.Key.Contains("name"))
            {
                result.FullName = keyValuePair.Value.ToString();
            }
            if (keyValuePair.Key.Contains("role"))
            {
                result.UserRoles = keyValuePair.Value.ToString();
            }
        }

        result.AuthData = new AuthenticationData();
        result.AuthData.JwtSecurityToken = token;
        result.AuthData.ExpirationDate = decodedValue.ValidTo;

        return result;
    }

    public async Task<User?> DoLoginAsync(LoginRequest userCredentials)
    {
        var authResponseResult = await AuthenticateAsync(userCredentials);
        if (authResponseResult.IsError)
        {
            return null;
        }

        return SetUserInfoDataFromAuthResponse(authResponseResult.Value.AccessToken);
    }
}
