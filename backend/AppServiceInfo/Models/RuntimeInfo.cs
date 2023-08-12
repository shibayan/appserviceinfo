namespace AppServiceInfo.Models;

public class RuntimeInfo
{
    public required VersionInfoList Dotnet { get; init; }

    public required VersionInfoList DotnetCore { get; init; }

    public required VersionInfoList DotnetCore64 { get; init; }

    public required VersionInfoList DotnetCoreSdk { get; init; }

    public required VersionInfoList DotnetCoreSdk64 { get; init; }

    public required VersionInfoList OracleJava { get; init; }

    public required VersionInfoList AzulJava { get; init; }

    public required VersionInfoList MicrosoftJava { get; init; }

    public required VersionInfoList Node { get; init; }

    public required VersionInfoList Node64 { get; init; }

    public required VersionInfoList Npm { get; init; }
}
