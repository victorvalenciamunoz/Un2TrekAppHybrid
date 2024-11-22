using System.Text.Json;
using Un2TrekApp.Authentication;
using Un2TrekApp.Domain;
using Un2TrekApp.Storage;

namespace Un2TrekApp.Activities;

internal class ActivitiesService : TokenServiceBase, IActivitiesService
{
    private readonly HttpClient _httpClient;

    public ActivitiesService(HttpClient httpClient, ILocalStorage localStorage) : base(localStorage, httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Activity>> GetActiveActivityListAsync()
    {
        var activityList = new List<Activity>();
        var url = $"";

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
            var objectResponse = JsonSerializer.Deserialize<List<ActivityResponse>>(json, options);
            if (objectResponse is not null)
            {
                foreach (var item in objectResponse)
                {
                    activityList.Add(new Activity
                    {
                        Id = item.Id,
                        Name = item.Title,
                        Description = item.Description
                    });
                }
            }
        }

        return activityList;
    }
}
