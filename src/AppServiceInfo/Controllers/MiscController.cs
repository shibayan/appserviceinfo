using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;

namespace AppServiceInfo.Controllers
{
    [Route("api/misc")]
    [ApiController]
    public class MiscController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var data = new MiscInfo
            {
                Bower = GetBowerVersions(),
                FSharp = GetFSharpVersions(),
                Grunt = GetGruntVersions(),
                Gulp = GetGulpVersions(),
                TypeScript = GetTypeScriptVersions(),
                MySql = GetMySqlVersions()
            };

            return Ok(data);
        }

        private static IReadOnlyList<VersionInfo> GetTypeScriptVersions()
        {
            var typeScriptDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"Microsoft SDKs\TypeScript");

            var list = Directory.EnumerateDirectories(typeScriptDirectory)
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetFSharpVersions()
        {
            var fsharpDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"Microsoft SDKs\F#");

            var list = Directory.EnumerateDirectories(fsharpDirectory)
                                .Where(x => !x.Contains("Licenses"))
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetBowerVersions()
        {
            var bowerDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"bower");

            var list = Directory.EnumerateDirectories(bowerDirectory)
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetGruntVersions()
        {
            var gruntDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"grunt");

            var list = Directory.EnumerateDirectories(gruntDirectory)
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetGulpVersions()
        {
            var gulpDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"gulp");

            var list = Directory.EnumerateDirectories(gulpDirectory)
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }

        private static IReadOnlyList<VersionInfo> GetMySqlVersions()
        {
            var mysqlDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), @"mysql");

            var list = Directory.EnumerateDirectories(mysqlDirectory)
                                .Where(x => !x.Contains("Connector"))
                                .Select(x => new VersionInfo(Path.GetFileName(x)))
                                .OrderBy(x => x.Version)
                                .ToArray();

            return list;
        }
    }
}
