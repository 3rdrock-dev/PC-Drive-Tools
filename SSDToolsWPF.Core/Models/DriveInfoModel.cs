// Developed for 3rdRock by Jim Barber (January 6, 2026)

namespace SSDToolsWPF.Core.Models;

public class DriveInfoModel
{
    public string Letter { get; set; } = string.Empty;
    public string FileSystem { get; set; } = string.Empty;
    public string Label { get; set; } = string.Empty;
    public string Health { get; set; } = string.Empty;

    public override string ToString() => Letter;
}