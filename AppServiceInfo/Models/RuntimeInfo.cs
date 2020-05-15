using System.Collections.Generic;

namespace AppServiceInfo.Models
{
    public class RuntimeInfo
    {
        public IReadOnlyList<VersionInfo> Dotnet { get; set; }

        public IReadOnlyList<VersionInfo> DotnetCore { get; set; }

        public IReadOnlyList<VersionInfo> DotnetCore64 { get; set; }

        public IReadOnlyList<VersionInfo> DotnetCoreSdk { get; set; }

        public IReadOnlyList<VersionInfo> DotnetCoreSdk64 { get; set; }

        public IReadOnlyList<VersionInfo> OracleJava { get; set; }

        public IReadOnlyList<VersionInfo> AzulJava { get; set; }

        public IReadOnlyList<VersionInfo> Node { get; set; }

        public IReadOnlyList<VersionInfo> Node64 { get; set; }

        public IReadOnlyList<VersionInfo> Npm { get; set; }

        public IReadOnlyList<VersionInfo> Php { get; set; }

        public IReadOnlyList<VersionInfo> Php64 { get; set; }

        public IReadOnlyList<VersionInfo> Python { get; set; }
    }
}