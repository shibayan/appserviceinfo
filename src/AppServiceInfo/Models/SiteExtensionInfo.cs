using System.Collections.Generic;

namespace AppServiceInfo.Models
{
    public class SiteExtensionInfo
    {
        public string Name { get; set; }

        public bool Enabled { get; set; }

        public IList<VersionInfo> Versions { get; set; }
    }
}