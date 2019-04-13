﻿using System.Collections.Generic;

using Newtonsoft.Json;

namespace AppServiceInfo.Models
{
    public class SiteExtensionInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("enabled")]
        public bool Enabled { get; set; }

        [JsonProperty("versions")]
        public IList<VersionInfo> Versions { get; set; }
    }
}