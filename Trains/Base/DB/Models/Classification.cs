namespace Trains.Base.DB.Models;

public sealed class Classification
{
    public Guid                            Id { get; set; }
    public required string                 Name { get; set; }
    public required string                 Code { get; set; }
    public required DateTime               LastUpdatedAt { get; set; }
    public required string                 LastUpdatedBy { get; set; }
    public required byte                   NvrServerQty { get; set; }
    public required byte                   SpsoServerQty { get; set; }
    public required short                  NoOfNvrSets { get; set; }
    public required decimal                TrainLengthInMeters { get; set; }
    public required int                    WeightInKg { get; set; }
    public required short                  MaxSpeedInKmph { get; set; }
    public required string                 StartIpRange { get; set; }
    public required string                 EndIpRange { get; set; }
    public required ClassificationCapacity Capacity { get; set; }

    #region Navigation properties

    public List<DeviceType>                DeviceTypes { get; set; } = [];

    #endregion
}
