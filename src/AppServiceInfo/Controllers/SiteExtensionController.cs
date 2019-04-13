using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;

namespace AppServiceInfo.Controllers
{
    [ApiController]
    [Route("api/siteextension")]
    public class SiteExtensionController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var siteExtensionDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "SiteExtensions");

            var list = Directory.EnumerateDirectories(siteExtensionDirectory)
                                .Select(x => new SiteExtensionInfo
                                {
                                    Name = Path.GetFileName(x),
                                    Enabled = IsSiteExtensionEnabled(x),
                                    Versions = GetSiteExtensionVersions(x)
                                })
                                .ToArray();

            return Ok(list);
        }

        private static bool IsSiteExtensionEnabled(string directory)
        {
            var isEnabled = true;

            var extensionPath = Path.Combine(directory, "extension.xml");

            if (System.IO.File.Exists(extensionPath))
            {
                var document = XElement.Load(extensionPath);

                isEnabled = (string)document.Element("version") != "disabled";
            }

            var environmentKey = $"{Path.GetFileName(directory)}_EXTENSION_VERSION";
            var environmentValue = Environment.GetEnvironmentVariable(environmentKey);

            if (!string.IsNullOrEmpty(environmentValue))
            {
                isEnabled = environmentValue != "disabled";
            }

            return isEnabled;
        }

        private static IList<VersionInfo> GetSiteExtensionVersions(string directory)
        {
            return Directory.EnumerateDirectories(directory)
                            .Select(x => new VersionInfo(Path.GetFileName(x).Replace("-", ".").Replace("beta", "0"), Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();
        }
    }
}
