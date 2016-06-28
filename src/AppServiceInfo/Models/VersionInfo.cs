using Newtonsoft.Json;

namespace AppServiceInfo.Models
{
    public class VersionInfo
    {
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}