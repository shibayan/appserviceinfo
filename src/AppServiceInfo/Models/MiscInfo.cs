using System.Collections.Generic;

using Newtonsoft.Json;

namespace AppServiceInfo.Models
{
    public class MiscInfo
    {
        [JsonProperty("typeScript")]
        public IEnumerable<VersionInfo> TypeScript { get; set; }

        [JsonProperty("fsharp")]
        public IEnumerable<VersionInfo> FSharp { get; set; }

        [JsonProperty("bower")]
        public IEnumerable<VersionInfo> Bower { get; set; }

        [JsonProperty("grunt")]
        public IEnumerable<VersionInfo> Grunt { get; set; }

        [JsonProperty("gulp")]
        public IEnumerable<VersionInfo> Gulp { get; set; }

        [JsonProperty("mysql")]
        public IEnumerable<VersionInfo> MySql { get; set; }
    }
}