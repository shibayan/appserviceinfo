<script setup lang="ts">
import { computed, ref } from 'vue'
import { SiteExtension, VersionInfoList, createEmptyPlatform, createEmptyRuntime } from '../types/Models'
import { formatRelativeTime, getNewestTimestamp } from '../utils/FormatDate'
import { allRegions, regionData, geographyColors } from '../constants/Regions'
import { fetchRegionData } from '../composables/useRegionData'

type SortKey = 'lastRapidUpdate' | 'lastReimage'

type TimelineEntry = {
  region: string
  displayName: string
  geography: string
  metadataUpdatedAt: string
  lastRapidUpdate: string
  lastReimage: string
  appServiceVersion: string
  kuduVersion: string
  middlewareModuleVersion: string
  dotnetVersion: string
  nodeVersion: string
  functionsVersion: string
}

function compareVersions(left: string, right: string): number {
  return left.localeCompare(right, undefined, { numeric: true, sensitivity: 'base' })
}

function getLatestVersion(list: VersionInfoList | undefined): string {
  const versions = [...(list?.latestVersions ?? []), ...(list?.allVersions ?? [])]
    .map(item => item.version)
    .filter(Boolean)

  return versions.sort((left, right) => compareVersions(right, left))[0] ?? ''
}

function getLatestVersionFromLists(...lists: Array<VersionInfoList | undefined>): string {
  return lists
    .map(list => getLatestVersion(list))
    .filter(Boolean)
    .sort((left, right) => compareVersions(right, left))[0] ?? ''
}

function getFunctionsVersion(siteExtensions: SiteExtension[]): string {
  return getLatestVersion(siteExtensions.find(x => x.name === 'Functions')?.installed)
}

const { platformMap, runtimeMap, siteExtensionsMap, platformUpdatedAtMap } = await fetchRegionData()

const sortKey = ref<SortKey>('lastRapidUpdate')

function getSortTime(entry: TimelineEntry, key: SortKey): number {
  const value = entry[key]

  if (!value) return 0

  const parsed = new Date(value).getTime()

  return Number.isNaN(parsed) ? 0 : parsed
}

const timelineEntries = computed<TimelineEntry[]>(() => {
  return allRegions
    .map((region) => {
      const platform = platformMap.get(region) ?? createEmptyPlatform()
      const runtime = runtimeMap.get(region) ?? createEmptyRuntime()
      const siteExtensions = siteExtensionsMap.get(region) ?? []

      return {
        region,
        displayName: regionData[region]?.displayName || region,
        geography: regionData[region]?.geography || 'Unknown',
        metadataUpdatedAt: platformUpdatedAtMap.get(region) ?? '',
        lastRapidUpdate: platform.lastRapidUpdate,
        lastReimage: platform.lastReimage,
        appServiceVersion: platform.appServiceVersion,
        kuduVersion: platform.kuduVersion,
        middlewareModuleVersion: platform.middlewareModuleVersion,
        dotnetVersion: getLatestVersionFromLists(runtime.dotnet, runtime.dotnetCore, runtime.dotnetCore64),
        nodeVersion: getLatestVersionFromLists(runtime.node, runtime.node64),
        functionsVersion: getFunctionsVersion(siteExtensions),
      }
    })
    .sort((left, right) => {
      const rightSortTime = getSortTime(right, sortKey.value)
      const leftSortTime = getSortTime(left, sortKey.value)

      if (rightSortTime !== leftSortTime) {
        return rightSortTime - leftSortTime
      }

      const leftSnapshotTime = left.metadataUpdatedAt ? new Date(left.metadataUpdatedAt).getTime() : 0
      const rightSnapshotTime = right.metadataUpdatedAt ? new Date(right.metadataUpdatedAt).getTime() : 0

      return rightSnapshotTime - leftSnapshotTime
    })
})

const snapshotUpdatedAt = computed(() => {
  return getNewestTimestamp(timelineEntries.value.map(entry => entry.metadataUpdatedAt))
})
</script>

<template>
  <div class="timeline-page">
    <div class="level timeline-header">
      <div class="level-left">
        <div>
          <div class="title-row mb-2">
            <h1 class="title mb-0 timeline-title">
              <span class="icon-text timeline-title-text">
                <span class="icon has-text-info timeline-title-icon"><i class="fa-solid fa-timeline"></i></span>
                <span>Version Timeline</span>
              </span>
            </h1>
            <span v-if="snapshotUpdatedAt" class="tag is-info is-light is-medium snapshot-badge">
              Snapshot {{ formatRelativeTime(snapshotUpdatedAt) }}
            </span>
          </div>
          <p class="subtitle is-6 mb-0 timeline-subtitle">
            Latest regional metadata snapshots, ordered by the selected update signal.
          </p>
        </div>
      </div>

      <div class="level-right">
        <div class="field sort-field">
          <label class="label is-small mb-0 sort-label">Sort By</label>
          <div class="control sort-radio-group" role="radiogroup" aria-label="Sort timeline by">
            <label class="sort-radio-option" :class="{ 'is-active': sortKey === 'lastRapidUpdate' }">
              <input v-model="sortKey" type="radio" name="timeline-sort" value="lastRapidUpdate">
              <span>Rapid Update</span>
            </label>
            <label class="sort-radio-option" :class="{ 'is-active': sortKey === 'lastReimage' }">
              <input v-model="sortKey" type="radio" name="timeline-sort" value="lastReimage">
              <span>Reimage</span>
            </label>
          </div>
        </div>
      </div>
    </div>

    <div class="card timeline-card">
      <div class="card-content">
      <div v-if="timelineEntries.length" class="timeline-list">
        <article v-for="entry in timelineEntries" :key="entry.region" class="timeline-item">
          <div class="timeline-marker">
            <span class="timeline-dot"></span>
          </div>
          <div class="timeline-panel">
            <div class="timeline-panel-header">
              <div>
                <RouterLink :to="{ name: 'Region', params: { region: entry.region } }" class="timeline-region-link">
                  {{ entry.displayName }}
                </RouterLink>
              </div>
              <span class="tag is-rounded" :class="geographyColors[entry.geography] || 'is-light'">
                {{ entry.geography }}
              </span>
            </div>

            <div class="timeline-version-tags">
              <span v-if="entry.appServiceVersion" class="tag is-link is-light">App Service {{ entry.appServiceVersion }}</span>
              <span v-if="entry.kuduVersion" class="tag is-link is-light">Kudu {{ entry.kuduVersion }}</span>
              <span v-if="entry.middlewareModuleVersion" class="tag is-info is-light">Middleware {{ entry.middlewareModuleVersion }}</span>
              <span v-if="entry.dotnetVersion" class="tag is-success is-light">.NET {{ entry.dotnetVersion }}</span>
              <span v-if="entry.nodeVersion" class="tag is-success is-light">Node.js {{ entry.nodeVersion }}</span>
              <span v-if="entry.functionsVersion" class="tag is-warning is-light">Functions {{ entry.functionsVersion }}</span>
            </div>

            <div class="timeline-meta">
              <span class="timeline-meta-item">Rapid Update: {{ formatRelativeTime(entry.lastRapidUpdate) || 'Unknown' }}</span>
              <span class="timeline-meta-item">Reimage: {{ formatRelativeTime(entry.lastReimage) || 'Unknown' }}</span>
            </div>
          </div>
        </article>
      </div>

      <p v-else class="has-text-grey-light has-text-centered py-5">
        No timeline data is available.
      </p>
      </div>
    </div>
  </div>
</template>

<style scoped>
.timeline-page {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.title-row {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 0.45rem;
}

h1.timeline-title.title {
  font-size: 1.72rem !important;
  line-height: 1.08 !important;
  letter-spacing: -0.015em;
}

.timeline-title-text {
  display: inline-flex;
  align-items: center;
  gap: 0.12rem;
}

.timeline-title-icon {
  margin-right: 0 !important;
  width: 1.35rem;
  height: 1.35rem;
  min-width: 1.35rem;
  font-size: 1.16rem !important;
}

.timeline-header {
  align-items: flex-end;
  margin-bottom: 0;
}

.snapshot-badge {
  white-space: nowrap;
  margin-left: 0.15rem;
  padding: 0.35em 0.7em;
  font-size: 0.82rem;
  line-height: 1.2;
}

.timeline-subtitle {
  color: #6b7280;
  font-size: 0.98rem;
  line-height: 1.45;
}

.sort-field {
  display: inline-flex;
  align-items: center;
  gap: 0.6rem;
  margin-right: 0.75rem;
  margin-bottom: 0;
}

.sort-field .label {
  color: #6b7280;
}

.sort-label {
  white-space: nowrap;
}

.sort-radio-group {
  display: inline-flex;
  align-items: center;
  gap: 0.35rem;
  padding: 0.18rem;
  border: 1px solid #dbe4f0;
  border-radius: 999px;
  background: #f8fbff;
}

.sort-radio-option {
  position: relative;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  min-width: 6.6rem;
  padding: 0.32rem 0.72rem;
  border-radius: 999px;
  color: #4b5563;
  cursor: pointer;
  font-size: 0.82rem;
  font-weight: 600;
  transition: background-color 0.15s ease, color 0.15s ease, box-shadow 0.15s ease;
}

.sort-radio-option input {
  position: absolute;
  opacity: 0;
  pointer-events: none;
}

.sort-radio-option.is-active {
  background: #ffffff;
  color: #0078d4;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.08);
}

.timeline-card {
  overflow: hidden;
}

.timeline-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.timeline-item {
  display: grid;
  grid-template-columns: 1.5rem minmax(0, 1fr);
  gap: 1rem;
}

.timeline-marker {
  position: relative;
  display: flex;
  justify-content: center;
}

.timeline-marker::after {
  content: '';
  position: absolute;
  top: 1.1rem;
  bottom: -1rem;
  width: 2px;
  background: linear-gradient(180deg, #bfdbfe 0%, #e5e7eb 100%);
}

.timeline-item:last-child .timeline-marker::after {
  display: none;
}

.timeline-dot {
  width: 0.85rem;
  height: 0.85rem;
  margin-top: 0.25rem;
  border-radius: 999px;
  background: linear-gradient(135deg, #0078d4 0%, #00a4ef 100%);
  box-shadow: 0 0 0 4px #deecf9;
}

.timeline-panel {
  padding: 1rem 1.1rem;
  border: 1px solid #e5e7eb;
  border-radius: 12px;
  background: linear-gradient(180deg, #ffffff 0%, #f8fbff 100%);
}

.timeline-panel-header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
  margin-bottom: 0.75rem;
}

.timeline-region-link {
  color: var(--azure-blue, #0078d4);
  font-size: 1.05rem;
  font-weight: 700;
  text-decoration: none;
}

.timeline-region-link:hover {
  color: #005a9e;
  text-decoration: underline;
}

.timeline-version-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.35rem;
  margin-bottom: 0.75rem;
}

.timeline-meta {
  display: flex;
  flex-wrap: wrap;
  gap: 0.75rem;
  color: #4b5563;
  font-size: 0.85rem;
}

.timeline-meta-item {
  display: inline-flex;
  align-items: center;
  gap: 0.25rem;
}

.geo-americas {
  background-color: #d83b01 !important;
  color: #fff !important;
}

.geo-europe {
  background-color: #0052cc !important;
  color: #fff !important;
}

.geo-middleeast {
  background-color: #c88a00 !important;
  color: #fff !important;
}

.geo-asiapacific {
  background-color: #00837b !important;
  color: #fff !important;
}

@media (max-width: 768px) {
  .timeline-header {
    align-items: stretch;
  }

  .timeline-header .level-right {
    width: 100%;
    display: flex;
    flex-direction: column;
    gap: 0.75rem;
  }

  .sort-field {
    display: flex;
    align-items: flex-start;
    flex-direction: column;
    gap: 0.35rem;
    margin-right: 0;
  }

  .sort-radio-group,
  .timeline-header .level-right {
    width: 100%;
  }

  .sort-radio-option {
    flex: 1;
    min-width: 0;
  }

  .timeline-item {
    grid-template-columns: 1rem minmax(0, 1fr);
    gap: 0.75rem;
  }

  .timeline-panel {
    padding: 0.9rem;
  }

  .title-row {
    align-items: flex-start;
  }

  h1.timeline-title.title {
    font-size: 1.5rem !important;
  }

  .timeline-title-icon {
    width: 1.18rem;
    height: 1.18rem;
    min-width: 1.18rem;
    font-size: 1.02rem !important;
  }

  .timeline-panel-header {
    flex-direction: column;
  }

  .timeline-meta {
    flex-direction: column;
    gap: 0.35rem;
  }
}
</style>
