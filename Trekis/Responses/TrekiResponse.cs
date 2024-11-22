namespace Un2TrekApp.Trekis;

internal class TrekiResponse
{
    public string TrekiId { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;
}
