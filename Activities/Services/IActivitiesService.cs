using Un2TrekApp.Domain;

namespace Un2TrekApp.Activities;

internal interface IActivitiesService
{
    Task<List<Activity>> GetActiveActivityListAsync();
}
