using ErrorOr;
using Un2TrekApp.Domain;

namespace Un2TrekApp.Trekis;

internal interface ITrekisService
{
    Task<ErrorOr<Success>> CaptureTreki(string activityId, Treki treki, (double Latitude, double Longitude) currentLocation, string userId);    
}