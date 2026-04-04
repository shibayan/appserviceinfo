import { Platform, Runtime, SiteExtension, createEmptyPlatform, createEmptyRuntime } from '../types/Models'
import { allRegions } from '../constants/Regions'
import { getUpdatedAt } from '../utils/FormatDate'

const METADATA_BASE_URL = 'https://stgraffias.blob.core.windows.net/metadata'

export type RegionData = {
  platformMap: Map<string, Platform>
  runtimeMap: Map<string, Runtime>
  siteExtensionsMap: Map<string, SiteExtension[]>
  platformUpdatedAtMap: Map<string, string>
}

export async function fetchRegionData(): Promise<RegionData> {
  const [platformEntries, runtimeEntries, siteExtensionEntries] = await Promise.all([
    Promise.all(
      allRegions.map(async (region): Promise<[string, Platform, string]> => {
        const response = await fetch(`${METADATA_BASE_URL}/${region}/platform.json`)

        if (!response.ok) {
          return [region, createEmptyPlatform(), '']
        }

        return [region, await response.json() as Platform, getUpdatedAt(response)]
      }),
    ),
    Promise.all(
      allRegions.map(async (region): Promise<[string, Runtime]> => {
        const response = await fetch(`${METADATA_BASE_URL}/${region}/runtime.json`)
        return [region, response.ok ? await response.json() as Runtime : createEmptyRuntime()]
      }),
    ),
    Promise.all(
      allRegions.map(async (region): Promise<[string, SiteExtension[]]> => {
        const response = await fetch(`${METADATA_BASE_URL}/${region}/site-extension.json`)
        return [region, response.ok ? await response.json() as SiteExtension[] : []]
      }),
    ),
  ])

  return {
    platformMap: new Map(platformEntries.map(([region, platform]) => [region, platform])),
    runtimeMap: new Map(runtimeEntries),
    siteExtensionsMap: new Map(siteExtensionEntries),
    platformUpdatedAtMap: new Map(platformEntries.map(([region, , updatedAt]) => [region, updatedAt])),
  }
}
