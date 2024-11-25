using ErrorOr;
using System.Net.Http.Json;
using System.Text.Json;
using Un2TrekApp.Authentication;
using Un2TrekApp.Domain;
using Un2TrekApp.Storage;

namespace Un2TrekApp.Trekis;

internal class TrekisService : TokenServiceBase, ITrekisService
{
    private readonly HttpClient _httpClient;
    public TrekisService(HttpClient httpClient, ILocalStorage localStorage) : base(localStorage, httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<ErrorOr<Success>> CaptureTreki(string activityId, Treki treki, (double Latitude, double Longitude) currentLocation, string userId)
    {
        var token = await this.GetTokenForCall();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CaptureTrekiRequest request = new CaptureTrekiRequest();
        request.Latitude = currentLocation.Latitude;
        request.Longitude = currentLocation.Longitude;
        request.TrekiId = treki.Id;
        request.ActivityId = activityId;
        request.UserId = userId;

        var url = "capture";

        var response = await _httpClient.PostAsJsonAsync(url, request);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success;
        }

        var problemDetailJson = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        var problemDetail = JsonSerializer.Deserialize<ProblemDetailResponse>(problemDetailJson, options);
        if (problemDetail?.Errors != null && problemDetail.Errors.Count > 0)
        {
            return Errors.GetErrorByCode(problemDetail.Errors.First().Key);

        }
        return Error.Unexpected(description: problemDetail?.Detail ?? "Unknown error");        
    }
}
