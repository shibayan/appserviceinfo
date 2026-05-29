export const METADATA_BASE_URL = 'https://stgraffias.blob.core.windows.net/metadata'

export type MetadataKind = 'platform' | 'runtime' | 'site-extension'

export function metadataUrl(region: string, kind: MetadataKind): string {
  return `${METADATA_BASE_URL}/${region}/${kind}.json`
}
