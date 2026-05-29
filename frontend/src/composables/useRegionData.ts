import { Platform, Runtime, SiteExtension, createEmptyPlatform, createEmptyRuntime } from '../types/Models'
import { allRegions } from '../constants/Regions'
import { metadataUrl } from '../constants/Metadata'
import { getUpdatedAt } from '../utils/FormatDate'

export type RegionData = {
  platformMap: Map<string, Platform>
  runtimeMap: Map<string, Runtime>
  siteExtensionsMap: Map<string, SiteExtension[]>
  platformUpdatedAtMap: Map<string, string>
}

// A single unreachable region (network error / invalid JSON) must not fail the whole
// snapshot, so each fetch degrades to empty data instead of rejecting.
async function fetchPlatform(region: string): Promise<[string, Platform, string]> {
  try {
    const response = await fetch(metadataUrl(region, 'platform'))

    if (!response.ok) {
      return [region, createEmptyPlatform(), '']
    }

    return [region, await response.json() as Platform, getUpdatedAt(response)]
  }
  catch {
    return [region, createEmptyPlatform(), '']
  }
}

async function fetchRuntime(region: string): Promise<[string, Runtime]> {
  try {
    const response = await fetch(metadataUrl(region, 'runtime'))
    return [region, response.ok ? await response.json() as Runtime : createEmptyRuntime()]
  }
  catch {
    return [region, createEmptyRuntime()]
  }
}

async function fetchSiteExtensions(region: string): Promise<[string, SiteExtension[]]> {
  try {
    const response = await fetch(metadataUrl(region, 'site-extension'))
    return [region, response.ok ? await response.json() as SiteExtension[] : []]
  }
  catch {
    return [region, []]
  }
}

export async function fetchRegionData(): Promise<RegionData> {
  const [platformEntries, runtimeEntries, siteExtensionEntries] = await Promise.all([
    Promise.all(allRegions.map(fetchPlatform)),
    Promise.all(allRegions.map(fetchRuntime)),
    Promise.all(allRegions.map(fetchSiteExtensions)),
  ])

  return {
    platformMap: new Map(platformEntries.map(([region, platform]) => [region, platform])),
    runtimeMap: new Map(runtimeEntries),
    siteExtensionsMap: new Map(siteExtensionEntries),
    platformUpdatedAtMap: new Map(platformEntries.map(([region, , updatedAt]) => [region, updatedAt])),
  }
}
