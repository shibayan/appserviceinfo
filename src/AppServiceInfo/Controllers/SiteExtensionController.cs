using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;

namespace AppServiceInfo.Controllers
{
    [Route("api/siteextension")]
    public class SiteExtensionController : Controller
    {
        public IActionResult Get()
        {
            var siteExtensionDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)"), "SiteExtensions");

            var list = Directory.EnumerateDirectories(siteExtensionDirectory)
                                .Select(x => new SiteExtensionInfo
                                {
                                    Name = Path.GetFileName(x),
                                    Enabled = IsSiteExtensionEnabled(x)
                                });

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
    }
}
