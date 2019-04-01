using System;

using Newtonsoft.Json;

namespace AppServiceInfo.Models
{
    public class VersionInfo
    {
        public VersionInfo(string version)
            : this(version, version)
        {
        }

        public VersionInfo(string version, string displayVersion)
        {
            Version = new Version(version);
            DisplayVersion = displayVersion;
        }

        [JsonIgnore]
        public Version Version { get; set; }

        [JsonProperty("version")]
        public string DisplayVersion { get; set; }
    }
}