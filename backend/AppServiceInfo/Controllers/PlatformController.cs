using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace AppServiceInfo.Controllers;

[Route("api/platform")]
[ApiController]
public class PlatformController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var info = new PlatformInfo
        {
            OsVersion = GetOSVersion(),
            AppServiceVersion = GetAppServiceVersion(),
            KuduVersion = GetKuduVersion(),
            MiddlewareModuleVersion = GetMiddlewareModuleVersion(),
            ProcessorName = GetProcessorName(),
            LastReimage = GetLastReimage(),
            LastRapidUpdate = GetLastRapidUpdate(),
            RegionName = Environment.GetEnvironmentVariable("REGION_NAME") ?? "Unknown region",
            CurrentStampname = Environment.GetEnvironmentVariable("WEBSITE_CURRENT_STAMPNAME") ?? "Unknown stampname",
            MachineName = Environment.MachineName
        };

        return Ok(info);
    }

    // ReSharper disable once InconsistentNaming
    private static string GetOSVersion()
    {
        using var currentVersionKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion")!;

        return $"{currentVersionKey.GetValue("ProductName")} (Build {currentVersionKey.GetValue("CurrentBuildNumber")}.{currentVersionKey.GetValue("UBR")})";
    }

    private static string GetAppServiceVersion()
    {
        var assemblyPath = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432")!, @"Reference Assemblies\Microsoft\IIS\Microsoft.Web.Hosting.dll");

        return FileVersionInfo.GetVersionInfo(assemblyPath).ProductVersion ?? "Unknown version";
    }

    private static string GetKuduVersion()
    {
        var kuduDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)")!, @"SiteExtensions\Kudu");

        return Directory.EnumerateDirectories(kuduDirectory)
                        .Select(Path.GetFileName)
                        .OrderByDescending(x => x)
                        .First()!;
    }

    private static string GetMiddlewareModuleVersion()
    {
        var middlewareDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)")!, "MiddlewareModules");

        return Directory.EnumerateDirectories(middlewareDirectory)
                        .Select(x => new Version(Path.GetFileName(x)))
                        .OrderByDescending(x => x)
                        .First().ToString();
    }

    private static string GetProcessorName()
    {
        using var processorKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0")!;

        return (string)processorKey.GetValue("ProcessorNameString")!;
    }

    private static DateTime? GetLastReimage()
    {
        var file = Directory.GetFiles($@"{Environment.GetEnvironmentVariable("SystemDrive")}\WebsitesInstall")
                            .Select(x => new FileInfo(x))
                            .Where(x => x.Length > 1024 * 1024)
                            .MaxBy(x => x.LastWriteTimeUtc);

        return file?.LastWriteTimeUtc;
    }

    private static DateTime? GetLastRapidUpdate()
    {
        var file = Directory.GetFiles($@"{Environment.GetEnvironmentVariable("SystemDrive")}\WebsitesInstall")
                            .Select(x => new FileInfo(x))
                            .Where(x => x.Length < 1024 * 1024)
                            .MaxBy(x => x.LastWriteTimeUtc);

        return file?.LastWriteTimeUtc;
    }
}
