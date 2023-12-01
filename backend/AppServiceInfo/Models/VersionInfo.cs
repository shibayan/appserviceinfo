using System;
using System.Text.Json.Serialization;

namespace AppServiceInfo.Models;

public class VersionInfo(string version, string displayVersion)
{
    public VersionInfo(string version)
        : this(version, version)
    {
    }

    [JsonIgnore]
    public Version Version { get; } = new(version);

    [JsonPropertyName("version")]
    public string DisplayVersion { get; } = displayVersion;
}
