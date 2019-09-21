using System.Collections.Generic;

namespace AppServiceInfo.Models
{
    public class MiscInfo
    {
        public IReadOnlyList<VersionInfo> TypeScript { get; set; }

        public IReadOnlyList<VersionInfo> FSharp { get; set; }

        public IReadOnlyList<VersionInfo> Bower { get; set; }

        public IReadOnlyList<VersionInfo> Grunt { get; set; }

        public IReadOnlyList<VersionInfo> Gulp { get; set; }

        public IReadOnlyList<VersionInfo> MySql { get; set; }
    }
}