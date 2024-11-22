namespace Un2TrekApp.Trekis;

internal class CaptureTrekiRequest
{
    public string ActivityId { get; set; }
    public double TrekiLatitude { get; set; }
    public double TrekiLongitude { get; set; }
    public double CurrentLatitude { get; set; }
    public double CurrentLongitude { get; set; }
    public string UserId { get; set; }
}
