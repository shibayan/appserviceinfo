using System.Collections.Generic;

using Newtonsoft.Json;

namespace AppServiceInfo.Models
{
    public class RuntimeInfo
    {
        [JsonProperty("dotnet")]
        public IReadOnlyList<VersionInfo> Dotnet { get; set; }

        [JsonProperty("dotnetCore")]
        public IReadOnlyList<VersionInfo> DotnetCore { get; set; }

        [JsonProperty("dotnetCore64")]
        public IReadOnlyList<VersionInfo> DotnetCore64 { get; set; }

        [JsonProperty("java")]
        public IReadOnlyList<VersionInfo> Java { get; set; }

        [JsonProperty("node")]
        public IReadOnlyList<VersionInfo> Node { get; set; }

        [JsonProperty("npm")]
        public IReadOnlyList<VersionInfo> Npm { get; set; }

        [JsonProperty("php")]
        public IReadOnlyList<VersionInfo> Php { get; set; }

        [JsonProperty("python")]
        public IReadOnlyList<VersionInfo> Python { get; set; }
    }
}