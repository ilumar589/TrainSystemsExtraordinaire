namespace Trains.Base.DB.Models;

public sealed class CameraSettings
{
    public int[]               SupportedFrames { get; set; }   = [30];
    public SupportedQuality[] SupportedQualities { get; set; } = [ SupportedQuality.Low ];
    public string?            FfmpegSettings { get; set; }     = null;

}
