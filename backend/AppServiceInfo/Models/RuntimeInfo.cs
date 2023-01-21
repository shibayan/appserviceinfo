using System.Collections.Generic;

namespace AppServiceInfo.Models;

public class RuntimeInfo
{
    public required IReadOnlyList<VersionInfo> Dotnet { get; init; }

    public required IReadOnlyList<VersionInfo> DotnetCore { get; init; }

    public required IReadOnlyList<VersionInfo> DotnetCore64 { get; init; }

    public required IReadOnlyList<VersionInfo> DotnetCoreSdk { get; init; }

    public required IReadOnlyList<VersionInfo> DotnetCoreSdk64 { get; init; }

    public required IReadOnlyList<VersionInfo> OracleJava { get; init; }

    public required IReadOnlyList<VersionInfo> AzulJava { get; init; }

    public required IReadOnlyList<VersionInfo> MicrosoftJava { get; init; }

    public required IReadOnlyList<VersionInfo> Node { get; init; }

    public required IReadOnlyList<VersionInfo> Node64 { get; init; }

    public required IReadOnlyList<VersionInfo> Npm { get; init; }
}
