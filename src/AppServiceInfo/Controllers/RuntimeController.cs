using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace AppServiceInfo.Controllers
{
    [Route("api/runtime")]
    public class RuntimeController : Controller
    {
        public IActionResult Get()
        {
            var data = new RuntimeInfo
            {
                Dotnet = GetDotnetVersions(),
                DotnetCore = GetDotnetCoreVersions(),
                Java = GetJavaVersions(),
                Node = GetNodeVersions(),
                Npm = GetNpmVersions(),
                Php = GetPhpVersions(),
                Python = GetPythonVersions()
            };

            return Ok(data);
        }

        private IEnumerable<VersionInfo> GetDotnetVersions()
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

                    list.Add(new VersionInfo
                    {
                        Version = versionKeyName.Substring(1)
                    });
                }
            }

            using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if (ndpKey?.GetValue("Release") != null)
                {
                    list.Add(new VersionInfo
                    {
                        Version = GetDotnet45Version((int)ndpKey.GetValue("Release"))
                    });
                }
            }

            return list;
        }

        private static string GetDotnet45Version(int releaseKey)
        {
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

        private static IEnumerable<VersionInfo> GetDotnetCoreVersions()
        {
            var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "dotnet", @"shared\Microsoft.NETCore.App");

            var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                                .Select(x => new VersionInfo
                                {
                                    Version = Path.GetFileName(x)
                                });

            return list;
        }

        private static IEnumerable<VersionInfo> GetJavaVersions()
        {
            var javaDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "Java");

            if (!Directory.Exists(javaDirectory))
            {
                return Enumerable.Empty<VersionInfo>();
            }

            var list = Directory.EnumerateDirectories(javaDirectory)
                                .Where(x => x.Contains("jdk"))
                                .Select(x => new VersionInfo
                                {
                                    Version = Path.GetFileName(x).Substring(3)
                                });

            return list;
        }

        private static IEnumerable<VersionInfo> GetNodeVersions()
        {
            var nodeDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "nodejs");

            var list = Directory.EnumerateDirectories(nodeDirectory)
                                .Where(x => !x.EndsWith("node_modules"))
                                .Select(x => new VersionInfo
                                {
                                    Version = Path.GetFileName(x)
                                });

            return list;
        }

        private static IEnumerable<VersionInfo> GetNpmVersions()
        {
            var npmDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "npm");

            var list = Directory.EnumerateDirectories(npmDirectory)
                                .Select(x => new VersionInfo
                                {
                                    Version = Path.GetFileName(x)
                                });

            return list;
        }

        private static IEnumerable<VersionInfo> GetPhpVersions()
        {
            var phpDirectory = Path.Combine(Environment.GetEnvironmentVariable("LOCAL_EXPANDED"), "Config");

            var list = Directory.EnumerateDirectories(phpDirectory, "PHP-*")
                                .Select(x => new VersionInfo
                                {
                                    Version = Path.GetFileName(x).Substring(4)
                                });

            return list;
        }

        private static IEnumerable<VersionInfo> GetPythonVersions()
        {
            var rootDirectory = Path.GetPathRoot(Environment.GetEnvironmentVariable("ProgramW6432"));

            var list = Directory.EnumerateDirectories(rootDirectory, "Python*")
                                .Select(x => new VersionInfo
                                {
                                    Version = System.IO.File.ReadLines(Path.Combine(x, "README.txt")).First().Substring(23)
                                });

            return list;
        }
    }
}
