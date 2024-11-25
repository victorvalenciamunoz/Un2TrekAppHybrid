namespace Un2TrekApp.Trekis;

internal class CaptureTrekiRequest
{
    public string ActivityId { get; set; }
    public string TrekiId { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string UserId { get; set; }
}
