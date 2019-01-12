using System.Collections.Generic;

using Newtonsoft.Json;

namespace AppServiceInfo.Models
{
    public class MiscInfo
    {
        [JsonProperty("typeScript")]
        public IReadOnlyList<VersionInfo> TypeScript { get; set; }

        [JsonProperty("fsharp")]
        public IReadOnlyList<VersionInfo> FSharp { get; set; }

        [JsonProperty("bower")]
        public IReadOnlyList<VersionInfo> Bower { get; set; }

        [JsonProperty("grunt")]
        public IReadOnlyList<VersionInfo> Grunt { get; set; }

        [JsonProperty("gulp")]
        public IReadOnlyList<VersionInfo> Gulp { get; set; }

        [JsonProperty("mysql")]
        public IReadOnlyList<VersionInfo> MySql { get; set; }
    }
}