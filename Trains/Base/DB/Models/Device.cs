namespace Trains.Base.DB.Models;

public sealed class Device
{
    public Guid                     Id { get; set; }
    public required string          IpAddress { get; set; }
    public required long            ViIdentifier { get; set; }
    public required DateTime        LastUpdatedAt { get; set; }
    public required string          LastUpdatedBy { get; set; }

    #region Navigation properties

    public required Guid            DeviceTypeId { get; set; }
    public required DeviceType      DeviceType { get; set; }

    #endregion
}
