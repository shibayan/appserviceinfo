using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                Java = GetJavaVersions(),
                Node = GetNodeVersions(),
                Node64 = GetNode64Versions(),
                Npm = GetNpmVersions(),
                Php = GetPhpVersions(),
                Php64 = GetPhp64Versions(),
                Python = GetPythonVersions()
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

        private static IReadOnlyList<VersionInfo> GetJavaVersions()
        {
            var javaDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles"), "Java");

            if (!Directory.Exists(javaDirectory))
            {
                return new VersionInfo[0];
            }

            var list = Directory.EnumerateDirectories(javaDirectory)
                                .Where(x => x.Contains("jdk") || x.Contains("jre"))
                                .Select(x =>
                                {
                                    var (version, displayVersion) = ExtractJavaVersion(Path.GetFileName(x));

                                    return new VersionInfo(version, displayVersion);
                                })
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static (string, string) ExtractJavaVersion(string directoryName)
        {
            if (directoryName.StartsWith("jdk") || directoryName.StartsWith("jre"))
            {
                return (directoryName.Substring(3).Replace("_", "."), $"{directoryName.Substring(3)} (Oracle)");
            }

            if (directoryName.StartsWith("zulu"))
            {
                var match = Regex.Match(directoryName, @"^.+?\-(jre|jdk)(\d+?)\.(\d+?)\.(\d+?)\-.*$");

                return ($"1.{match.Groups[2].Value}.{match.Groups[3].Value}.{match.Groups[4].Value}", $"1.{match.Groups[2].Value}.{match.Groups[3].Value}_{match.Groups[4].Value} (Azul)");
            }

            return (null, null);
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

        private static IReadOnlyList<VersionInfo> GetPythonVersions()
        {
            var rootDirectory = Path.GetPathRoot(Environment.GetEnvironmentVariable("ProgramW6432"));

            var list = Directory.EnumerateDirectories(rootDirectory, "Python*")
                                .Select(x => new VersionInfo(System.IO.File.Exists(Path.Combine(x, "README.txt"))
                                    ? System.IO.File.ReadLines(Path.Combine(x, "README.txt")).First().Substring(23)
                                    : FileVersionInfo.GetVersionInfo(Path.Combine(x, "python.exe")).ProductVersion))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }
    }
}
