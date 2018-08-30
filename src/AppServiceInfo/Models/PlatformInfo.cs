using System;

using Newtonsoft.Json;

namespace AppServiceInfo.Models
{
    public class PlatformInfo
    {
        [JsonProperty("osVersion")]
        public string OsVersion { get; set; }

        [JsonProperty("appServiceVersion")]
        public string AppServiceVersion { get; set; }

        [JsonProperty("kuduVersion")]
        public string KuduVersion { get; set; }

        [JsonProperty("processorName")]
        public string ProcessorName { get; set; }

        [JsonProperty("lastReimage")]
        public DateTime? LastReimage { get; set; }

        [JsonProperty("currentStampname")]
        public string CurrentStampname { get; set; }
    }
}