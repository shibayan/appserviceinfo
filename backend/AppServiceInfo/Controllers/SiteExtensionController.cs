using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

using AppServiceInfo.Models;

using Microsoft.AspNetCore.Mvc;

namespace AppServiceInfo.Controllers;

[Route("api/site-extension")]
[ApiController]
public class SiteExtensionController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var siteExtensionDirectory = Path.Combine(Environment.GetEnvironmentVariable("ProgramFiles(x86)")!, "SiteExtensions");

        var list = Directory.EnumerateDirectories(siteExtensionDirectory)
                            .Select(x => new SiteExtensionInfo
                            {
                                Name = Path.GetFileName(x),
                                Enabled = IsSiteExtensionEnabled(x),
                                Installed = GetSiteExtensionVersions(x)
                            })
                            .Where(x => x.Installed.AllVersions.Count > 0)
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

            isEnabled = (string?)document.Element("version") != "disabled";
        }

        var environmentKey = $"{Path.GetFileName(directory)}_EXTENSION_VERSION";
        var environmentValue = Environment.GetEnvironmentVariable(environmentKey);

        if (!string.IsNullOrEmpty(environmentValue))
        {
            isEnabled = environmentValue != "disabled";
        }

        return isEnabled;
    }

    private static VersionInfoList GetSiteExtensionVersions(string directory)
    {
        var list = Directory.EnumerateDirectories(directory)
                            .Where(x => !Path.GetFileName(x).StartsWith("_"))
                            .Select(x => new VersionInfo(Regex.Replace(Path.GetFileName(x), @"\-.*$", ""), Path.GetFileName(x)))
                            .OrderBy(x => x.Version)
                            .ToArray();

        return new VersionInfoList(list);
    }
}
