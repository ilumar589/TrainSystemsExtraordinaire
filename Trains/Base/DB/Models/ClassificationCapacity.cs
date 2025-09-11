namespace Trains.Base.DB.Models;

public sealed class ClassificationCapacity
{
    public required short SeatedCapacity { get; set; }
    public required short StandingCapacity { get; set; }
    public required short TotalCapacity { get; set; }

}
