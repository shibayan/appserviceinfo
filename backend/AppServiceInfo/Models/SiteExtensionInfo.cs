namespace AppServiceInfo.Models;

public class SiteExtensionInfo
{
    public required string Name { get; init; }

    public required bool Enabled { get; init; }

    public required VersionInfoList Installed { get; init; }
}
