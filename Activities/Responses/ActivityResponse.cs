namespace Un2TrekApp.Activities;

internal class ActivityResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime ValidFromDate { get; set; }

    public DateTime? ValidToDate { get; set; }
}
