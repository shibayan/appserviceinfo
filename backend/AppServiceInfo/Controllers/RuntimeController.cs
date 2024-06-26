﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;

namespace AppServiceInfo.Controllers;

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
            MicrosoftJava = GetMicrosoftJavaVersions(),
            Node = GetNodeVersions(),
            Node64 = GetNode64Versions(),
            Npm = GetNpmVersions()
        };

        return Ok(data);
    }

    private static VersionInfoList GetDotnetVersions()
    {
        var list = new List<VersionInfo>();

        using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\")!)
        {
            foreach (var versionKeyName in ndpKey.GetSubKeyNames().Where(x => x.StartsWith("v")))
            {
                var versionKey = ndpKey.OpenSubKey(versionKeyName)!;

                var version = (string?)versionKey.GetValue("Version");

                if (version is null)
                {
                    continue;
                }

                list.Add(new VersionInfo(versionKeyName[1..]));
            }
        }

        using (var ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
        {
            if (ndpKey?.GetValue("Release") is not null)
            {
                list.Add(new VersionInfo(GetDotnet45Version((int)ndpKey.GetValue("Release")!)));
            }
        }

        return new VersionInfoList(list);
    }

    private static string GetDotnet45Version(int releaseKey)
    {
        return releaseKey switch
        {
            >= 533320 => "4.8.1",
            >= 528040 => "4.8",
            >= 461808 => "4.7.2",
            >= 461308 => "4.7.1",
            >= 460798 => "4.7",
            >= 394802 => "4.6.2",
            >= 394254 => "4.6.1",
            >= 393295 => "4.6",
            >= 379893 => "4.5.2",
            >= 378675 => "4.5.1",
            _ => "4.5"
        };
    }

    private static VersionInfoList GetDotnetCoreVersions()
    {
        var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)")!, "dotnet", @"shared\Microsoft.NETCore.App");

        var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                            .Select(x => new VersionInfo(Regex.Replace(Path.GetFileName(x), @"\-.*$", ""), Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetDotnetCore64Versions()
    {
        var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432")!, "dotnet", @"shared\Microsoft.NETCore.App");

        var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                            .Select(x => new VersionInfo(Regex.Replace(Path.GetFileName(x), @"\-.*$", ""), Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetDotnetCoreSdkVersions()
    {
        var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)")!, "dotnet", "sdk");

        var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                            .Where(x => !x.EndsWith("NuGetFallbackFolder"))
                            .Select(x => new VersionInfo(Regex.Replace(Path.GetFileName(x), @"\-.*$", ""), Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetDotnetCoreSdk64Versions()
    {
        var dotnetCoreDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432")!, "dotnet", "sdk");

        var list = Directory.EnumerateDirectories(dotnetCoreDirectory)
                            .Where(x => !x.EndsWith("NuGetFallbackFolder"))
                            .Select(x => new VersionInfo(Regex.Replace(Path.GetFileName(x), @"\-.*$", ""), Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetOracleJavaVersions()
    {
        var javaDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432")!, "Java");

        if (!Directory.Exists(javaDirectory))
        {
            return new VersionInfoList([]);
        }

        var list = Directory.EnumerateDirectories(javaDirectory)
                            .Where(x => x.Contains("\\jdk") || x.Contains("\\jre"))
                            .Select(x => new VersionInfo(Path.GetFileName(x)[3..].Replace("_", ".")))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetAzulJavaVersions()
    {
        var javaDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432")!, "Java");

        if (!Directory.Exists(javaDirectory))
        {
            return new VersionInfoList([]);
        }

        var list = Directory.EnumerateDirectories(javaDirectory)
                            .Where(x => x.Contains("\\zulu"))
                            .Select(x =>
                            {
                                var match = Regex.Match(Path.GetFileName(x), @"^.+?\-(jre|jdk)(\d+?)\.(\d+?)\.(\d+?)\-.*$");

                                return new VersionInfo($"1.{match.Groups[2].Value}.{match.Groups[3].Value}.{match.Groups[4].Value}");
                            })
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetMicrosoftJavaVersions()
    {
        var javaDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432")!, "Java");

        if (!Directory.Exists(javaDirectory))
        {
            return new VersionInfoList([]);
        }

        var list = Directory.EnumerateDirectories(javaDirectory)
                            .Where(x => x.Contains("\\microsoft"))
                            .Select(x =>
                            {
                                var match = Regex.Match(Path.GetFileName(x), @"^.+?\-(jdk)\-(\d+?)\.(\d+?)\.(\d+?)\..*$");

                                return new VersionInfo($"1.{match.Groups[2].Value}.{match.Groups[3].Value}.{match.Groups[4].Value}");
                            })
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetNodeVersions()
    {
        var nodeDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)")!, "nodejs");

        var list = Directory.EnumerateDirectories(nodeDirectory)
                            .Where(x => !x.EndsWith("node_modules"))
                            .Select(x => new VersionInfo(Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetNode64Versions()
    {
        var nodeDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramW6432")!, "nodejs");

        var list = Directory.EnumerateDirectories(nodeDirectory)
                            .Where(x => !x.EndsWith("node_modules"))
                            .Select(x => new VersionInfo(Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }

    private static VersionInfoList GetNpmVersions()
    {
        var npmDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)")!, "npm");

        var list = Directory.EnumerateDirectories(npmDirectory)
                            .Select(x => new VersionInfo(Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }
}
