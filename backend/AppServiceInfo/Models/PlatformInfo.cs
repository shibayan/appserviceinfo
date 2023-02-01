using System;

namespace AppServiceInfo.Models;

public class PlatformInfo
{
    public required string OsVersion { get; init; }

    public required string AppServiceVersion { get; init; }

    public required string KuduVersion { get; init; }

    public required string MiddlewareModuleVersion { get; init; }

    public required string ProcessorName { get; init; }

    public required DateTime? LastReimage { get; init; }

    public required DateTime? LastRapidUpdate { get; init; }

    public required string RegionName { get; set; }

    public required string CurrentStampname { get; init; }

    public required string MachineName { get; set; }
}
