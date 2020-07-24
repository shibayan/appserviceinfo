using System;

namespace AppServiceInfo.Models
{
    public class PlatformInfo
    {
        public string OsVersion { get; set; }

        public string AppServiceVersion { get; set; }

        public string MiddlewareModuleVersion { get; set; }

        public string KuduVersion { get; set; }

        public string ProcessorName { get; set; }

        public DateTime? LastReimage { get; set; }

        public string CurrentStampname { get; set; }
    }
}