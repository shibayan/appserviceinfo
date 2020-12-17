using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace AppServiceInfo.Controllers
{
    [Route("api/runtime")]
    [ApiController]
    public class RuntimeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var data = new RuntimeInfo
            {
                Dotnet = GetDotnetVersions(),
                DotnetCore = GetDotnetCoreVersions(),
                DotnetCore64 = GetDotnetCore64Versions(),
                DotnetCoreSdk = GetDotnetCoreSdkVersions(),
                DotnetCoreSdk64 = GetDotnetCoreSdk64Versions(),
                OracleJava = GetOracleJavaVersions(),
                AzulJava = GetAzulJavaVersions(),
                Node = GetNodeVersions(),
                Node64 = GetNode64Versions(),
                Npm = GetNpmVersions(),
                Php = GetPhpVersions(),
                Php64 = GetPhp64Versions()
            };

            return Ok(data);
        }

        private IReadOnlyList<VersionInfo> GetDotnetVersions()
        {
            var list = new List<VersionInfo>();

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                foreach (var versionKeyName in ndpKey.GetSubKeyNames().Where(x => x.StartsWith("v")))
                {
                    var versionKey = ndpKey.OpenSubKey(versionKeyName);

                    var version = (string)versionKey.GetValue("Version");

                    if (version == null)
                    {
                        continue;
                    }

                    list.Add(new VersionInfo(versionKeyName.Substring(1)));
                }
            }

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if (ndpKey?.GetValue("Release") != null)
                {
                    list.Add(new VersionInfo(GetDotnet45Version((int)ndpKey.GetValue("Release"))));
                }
            }

            return list;
        }

        private static string GetDotnet45Version(int releaseKey)
        {
            if (releaseKey >= 528040)
            {
                return "4.8";
            }

            if (releaseKey >= 461808)
            {
                return "4.7.2";
            }

            if (releaseKey >= 461308)
            {
                return "4.7.1";
            }

            if (releaseKey >= 460798)
            {
                return "4.7";
            }

            if (releaseKey >= 394802)
            {
                return "4.6.2";
            }

            if (releaseKey >= 394254)
            {
                return "4.6.1";
            }

            if (releaseKey >= 393295)
            {
                return "4.6";
            }

            if (releaseKey >= 379893)
            {
                return "4.5.2";
            }

            if (releaseKey >= 378675)
            {
                return "4.5.1";
            }

            return "4.5";
        }

        private static IReadOnlyList<VersionInfo> GetDotnetCoreVersions()
        {
            var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "dotnet", @"shared\Microsoft.NETCore.App");

            var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetDotnetCore64Versions()
        {
            var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "dotnet", @"shared\Microsoft.NETCore.App");

            var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetDotnetCoreSdkVersions()
        {
            var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "dotnet", "sdk");

            var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                                .Where(x => !x.EndsWith("NuGetFallbackFolder"))
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetDotnetCoreSdk64Versions()
        {
            var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "dotnet", "sdk");

            var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                                .Where(x => !x.EndsWith("NuGetFallbackFolder"))
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetOracleJavaVersions()
        {
            var javaDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "Java");

            if (!Directory.Exists(javaDirectory))
            {
                return new VersionInfo[0];
            }

            var list = Directory.EnumerateDirectories(javaDirectory)
                                .Where(x => !x.Contains("zulu") && (x.Contains("jdk") || x.Contains("jre")))
                                .Select(x => new VersionInfo(Path.GetFileName(x).Substring(3).Replace("_", ".")))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetAzulJavaVersions()
        {
            var javaDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "Java");

            if (!Directory.Exists(javaDirectory))
            {
                return new VersionInfo[0];
            }

            var list = Directory.EnumerateDirectories(javaDirectory)
                                .Where(x => x.Contains("zulu"))
                                .Select(x =>
                                {
                                    var match = Regex.Match(Path.GetFileName(x), @"^.+?\-(jre|jdk)(\d+?)\.(\d+?)\.(\d+?)\-.*$");

                                    return new VersionInfo($"1.{match.Groups[2].Value}.{match.Groups[3].Value}.{match.Groups[4].Value}");
                                })
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetNodeVersions()
        {
            var nodeDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "nodejs");

            var list = Directory.EnumerateDirectories(nodeDirectory)
                                .Where(x => !x.EndsWith("node_modules"))
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetNode64Versions()
        {
            var nodeDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "nodejs");

            var list = Directory.EnumerateDirectories(nodeDirectory)
                                .Where(x => !x.EndsWith("node_modules"))
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetNpmVersions()
        {
            var npmDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "npm");

            var list = Directory.EnumerateDirectories(npmDirectory)
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetPhpVersions()
        {
            var phpDirectory = Path.Combine(Environment.GetEnvironmentVariable("LOCAL_EXPANDED"), "Config");

            var list = Directory.EnumerateDirectories(phpDirectory, "PHP-*")
                                .Where(x => !x.Contains("x64"))
                                .Select(x => new VersionInfo(Path.GetFileName(x).Substring(4)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }
        private static IReadOnlyList<VersionInfo> GetPhp64Versions()
        {
            var phpDirectory = Path.Combine(Environment.GetEnvironmentVariable("LOCAL_EXPANDED"), "Config");

            var list = Directory.EnumerateDirectories(phpDirectory, "PHP-*")
                                .Where(x => x.Contains("x64"))
                                .Select(x => x.Replace("x64", ""))
                                .Select(x => new VersionInfo(Path.GetFileName(x).Substring(4)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }
    }
}
