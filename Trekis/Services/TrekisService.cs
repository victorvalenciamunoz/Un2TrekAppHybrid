using ErrorOr;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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
    public async Task<List<Treki>> GetTrekiListByActivityAsync(string activityId)
    {
        var trekiList = new List<Treki>();
        var url = $"Un2TrekActividad/{activityId}/trekilist";

        var token = await this.GetTokenForCall();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var json = await response.Content.ReadAsStringAsync();
            var objectResponse = JsonSerializer.Deserialize<List<TrekiResponse>>(json, options);
            if (objectResponse is not null)
            {
                foreach (var item in objectResponse)
                {
                    trekiList.Add(new Treki
                    {
                        Id = item.TrekiId,
                        Latitude = item.Latitude,
                        Longitude = item.Longitude,
                        Description = item.Description,
                        Title = item.Title
                    });
                }
            }
        }
        return trekiList;
    }

    public async Task<ErrorOr<Success>> CaptureTreki(string activityId, Treki treki, (double Latitude, double Longitude) currentLocation, string userId)
    {
        var token = await this.GetTokenForCall();
        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        CaptureTrekiRequest request = new CaptureTrekiRequest();
        request.TrekiLatitude = treki.Latitude;
        request.TrekiLongitude = treki.Longitude;
        request.CurrentLatitude = currentLocation.Latitude;
        request.CurrentLongitude = currentLocation.Longitude;
        request.ActivityId = activityId;
        request.UserId = userId;

        var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        var url = "Un2TrekTreki/capture";
        
        var response = await _httpClient.PostAsJsonAsync(url, request);

        if (response.IsSuccessStatusCode)
        {
            return Result.Success;
        }
        
        var problemDetailJson = await response.Content.ReadAsStringAsync();

        var problemDetail = JsonSerializer.Deserialize<ProblemDetailResponse>(problemDetailJson);

        return Errors.GetErrorByCode(problemDetail.Code);
    }
}
