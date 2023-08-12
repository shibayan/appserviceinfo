using System.Collections.Generic;
using System.Linq;

namespace AppServiceInfo.Models;

public class VersionInfoList
{
    public VersionInfoList(IReadOnlyList<VersionInfo> allVersions)
    {
        LatestVersions = allVersions.GroupBy(x => x.Version.Major)
                                    .Select(x => x.OrderByDescending(xs => xs.Version).First())
                                    .ToArray();

        AllVersions = allVersions;
    }

    public IReadOnlyList<VersionInfo> LatestVersions { get; }

    public IReadOnlyList<VersionInfo> AllVersions { get; }
}
