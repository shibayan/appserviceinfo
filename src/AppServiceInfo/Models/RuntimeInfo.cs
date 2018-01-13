using System.Collections.Generic;

using Newtonsoft.Json;

namespace AppServiceInfo.Models
{
    public class RuntimeInfo
    {
        [JsonProperty("dotnet")]
        public IEnumerable<VersionInfo> Dotnet { get; set; }

        [JsonProperty("dotnetCore")]
        public IEnumerable<VersionInfo> DotnetCore { get; set; }

        [JsonProperty("java")]
        public IEnumerable<VersionInfo> Java { get; set; }

        [JsonProperty("node")]
        public IEnumerable<VersionInfo> Node { get; set; }

        [JsonProperty("npm")]
        public IEnumerable<VersionInfo> Npm { get; set; }

        [JsonProperty("php")]
        public IEnumerable<VersionInfo> Php { get; set; }

        [JsonProperty("python")]
        public IEnumerable<VersionInfo> Python { get; set; }
    }
}