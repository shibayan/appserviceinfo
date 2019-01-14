using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace AppServiceInfo.Controllers
{
    [ApiController]
    [Route("api/platform")]
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
                LastReimage = GetLastReimage(),
                CurrentStampname = Environment.GetEnvironmentVariable("WEBSITE_CURRENT_STAMPNAME"),
                ProcessorName = GetProcessorName()
            };

            return Ok(info);
        }

        private static string GetOSVersion()
        {
            using (var currentVersionKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion"))
            {
                return $"{currentVersionKey.GetValue("ProductName")} (Build {currentVersionKey.GetValue("CurrentBuildNumber")}.{currentVersionKey.GetValue("UBR")})";
            }
        }

        private static string GetKuduVersion()
        {
            var kuduDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"SiteExtensions\Kudu");

            return Directory.EnumerateDirectories(kuduDirectory)
                            .Select(Path.GetFileName)
                            .OrderByDescending(x => x)
                            .First();
        }

        private static string GetAppServiceVersion()
        {
            try
            {
                var assemblyPath = @"D:\Program Files\Reference Assemblies\Microsoft\IIS\Microsoft.Web.Hosting.dll";

                return FileVersionInfo.GetVersionInfo(assemblyPath).ProductVersion;
            }
            catch
            {
                return null;
            }
        }

        private static DateTime? GetLastReimage()
        {
            var file = Directory.GetFiles(@"D:\WebsitesInstall").FirstOrDefault();

            if (file == null)
            {
                return null;
            }

            return new FileInfo(file).CreationTimeUtc;
        }

        private static string GetProcessorName()
        {
            using (var processorKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Default).OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0"))
            {
                return (string)processorKey.GetValue("ProcessorNameString");
            }
        }
    }
}
