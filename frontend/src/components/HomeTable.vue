<script setup lang="ts">
import { ref, computed } from 'vue'
import { createEmptyPlatform, createEmptyRuntime } from '../types/Models'
import { formatRelativeTime, getNewestTimestamp } from '../utils/FormatDate'
import { geographies, regionData, geographyColors } from '../constants/Regions'
import { fetchRegionData } from '../composables/useRegionData'

const emit = defineEmits<{
  snapshotResolved: [value: string]
}>()

const selectedGeography = ref<string | null>(null)

const filteredGeographies = computed(() => {
  if (selectedGeography.value === null) return geographies
  return geographies.filter(g => g.name === selectedGeography.value)
})

const filteredRegions = computed(() => {
  return filteredGeographies.value.flatMap(g => g.regions)
})

const filteredPlatformList = computed(() => {
  return filteredRegions.value.map(region => platformMap.get(region) ?? createEmptyPlatform())
})

const filteredRuntimeList = computed(() => {
  return filteredRegions.value.map(region => runtimeMap.get(region) ?? createEmptyRuntime())
})

const filteredSiteExtensionsList = computed(() => {
  return filteredRegions.value.map(region => siteExtensionsMap.get(region) ?? [])
})

const { platformMap, runtimeMap, siteExtensionsMap, platformUpdatedAtMap } = await fetchRegionData()

const snapshotUpdatedAt = getNewestTimestamp([...platformUpdatedAtMap.values()])

if (snapshotUpdatedAt) {
  emit('snapshotResolved', snapshotUpdatedAt)
}
</script>

<template>
  <div class="geography-tabs">
    <div class="tab-bar">
      <button class="tab-btn" :class="{ 'tab-active': selectedGeography === null }"
        @click="selectedGeography = null">
        <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-solid fa-globe"></i></span>
        <span>All</span>
      </button>
      <button v-for="geography in geographies" :key="geography.name" class="tab-btn"
        :class="{ 'tab-active': selectedGeography === geography.name }"
        @click="selectedGeography = geography.name">
        <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-solid fa-earth-americas" v-if="geography.name === 'Americas'"></i><i class="fa-solid fa-earth-europe" v-else-if="geography.name === 'Europe' || geography.name === 'Middle East'"></i><i class="fa-solid fa-earth-asia" v-else></i></span>
        <span>{{ geography.name }}</span>
        <span class="tag tag-rounded tag-sm ml-1" :class="geographyColors[geography.name]">{{ geography.regions.length }}</span>
      </button>
    </div>
  </div>

  <div class="home-table-wrapper overflow-x-auto">
    <table class="home-table w-full border-collapse">
      <thead>
        <tr>
          <th rowspan="2" class="sticky-col header-col">
            <span class="inline-flex items-center gap-1">
              <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-solid fa-globe"></i></span>
              <span>Region</span>
            </span>
          </th>
          <th v-for="geography in filteredGeographies" :key="geography.name" :colspan="geography.regions.length"
            class="text-center geo-header" :class="geographyColors[geography.name]">
            {{ geography.name }}
          </th>
        </tr>
        <tr>
          <th v-for="region in filteredRegions" :key="region" class="text-center region-header">
            <RouterLink :to="{ name: 'Region', params: { region: region } }" class="region-link">
              {{ regionData[region]?.displayName || region }}
            </RouterLink>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr class="section-row">
          <th class="sticky-col" :colspan="filteredRegions.length + 1">
            <span class="inline-flex items-center gap-1">
              <span class="inline-flex items-center justify-center w-5 h-5 text-blue-600"><i class="fa-solid fa-server"></i></span>
              <span>Platform</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">OS Version</th>
          <td v-for="(platform, i) in filteredPlatformList" :key="filteredRegions[i] + '-os'" class="text-xs">{{ platform.osVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">App Service Version</th>
          <td v-for="(platform, i) in filteredPlatformList" :key="filteredRegions[i] + '-as'" class="text-xs">{{ platform.appServiceVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Kudu Version</th>
          <td v-for="(platform, i) in filteredPlatformList" :key="filteredRegions[i] + '-kudu'" class="text-xs">{{ platform.kuduVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Middleware Module</th>
          <td v-for="(platform, i) in filteredPlatformList" :key="filteredRegions[i] + '-mw'" class="text-xs">{{ platform.middlewareModuleVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Processor Name</th>
          <td v-for="(platform, i) in filteredPlatformList" :key="filteredRegions[i] + '-cpu'" class="text-xs">{{ platform.processorName }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Last Reimage</th>
          <td v-for="(platform, i) in filteredPlatformList" :key="filteredRegions[i] + '-ri'" class="text-xs">{{ formatRelativeTime(platform.lastReimage) }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Last Rapid Update</th>
          <td v-for="(platform, i) in filteredPlatformList" :key="filteredRegions[i] + '-ru'" class="text-xs">{{ formatRelativeTime(platform.lastRapidUpdate) }}
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Stampname</th>
          <td v-for="(platform, i) in filteredPlatformList" :key="filteredRegions[i] + '-stamp'" class="text-xs">{{ platform.currentStampname }}</td>
        </tr>

        <tr class="section-row">
          <th class="sticky-col" :colspan="filteredRegions.length + 1">
            <span class="inline-flex items-center gap-1">
              <span class="inline-flex items-center justify-center w-5 h-5 text-green-600"><i class="fa-solid fa-code"></i></span>
              <span>Runtime</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">.NET Framework</th>
          <td v-for="(runtime, i) in filteredRuntimeList" :key="filteredRegions[i] + '-dotnet'">
            <span v-for="item in runtime.dotnet.latestVersions" :key="item.version" class="tag tag-sm tag-link">{{ item.version
              }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">.NET (x86)</th>
          <td v-for="(runtime, i) in filteredRuntimeList" :key="filteredRegions[i] + '-dotnetcore'">
            <span v-for="item in runtime.dotnetCore.latestVersions" :key="item.version" class="tag tag-sm tag-link">{{
              item.version }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">.NET (x64)</th>
          <td v-for="(runtime, i) in filteredRuntimeList" :key="filteredRegions[i] + '-dotnetcore64'">
            <span v-for="item in runtime.dotnetCore64.latestVersions" :key="item.version" class="tag tag-sm tag-link">{{
              item.version }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Node.js (x86)</th>
          <td v-for="(runtime, i) in filteredRuntimeList" :key="filteredRegions[i] + '-node'">
            <span v-for="item in runtime.node.latestVersions" :key="item.version" class="tag tag-sm tag-success">{{ item.version
              }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Node.js (x64)</th>
          <td v-for="(runtime, i) in filteredRuntimeList" :key="filteredRegions[i] + '-node64'">
            <span v-for="item in runtime.node64.latestVersions" :key="item.version" class="tag tag-sm tag-success">{{
              item.version }}</span>
          </td>
        </tr>

        <tr class="section-row">
          <th class="sticky-col" :colspan="filteredRegions.length + 1">
            <span class="inline-flex items-center gap-1">
              <span class="inline-flex items-center justify-center w-5 h-5 text-amber-700"><i class="fa-solid fa-puzzle-piece"></i></span>
              <span>Site Extensions</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">Functions</th>
          <td v-for="(siteExtensions, i) in filteredSiteExtensionsList" :key="filteredRegions[i] + '-func'">
            <span v-for="item in siteExtensions.find(x => x.name === 'Functions')?.installed.latestVersions" :key="item.version"
              class="tag tag-sm tag-warning">{{ item.version }}</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.geography-tabs {
  margin-bottom: 0;
}

.tab-bar {
  display: flex;
  border-bottom: 1px solid #dbdbdb;
}

.tab-btn {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  padding: 0.5em 1em;
  font-size: 0.9rem;
  border: 1px solid transparent;
  border-bottom: 2px solid transparent;
  margin-bottom: -1px;
  background: none;
  color: #4a4a4a;
  cursor: pointer;
  transition: color 0.15s, border-color 0.15s;
}

.tab-btn:hover {
  color: #0078d4;
}

.tab-btn.tab-active {
  border: 1px solid #dbdbdb;
  border-bottom-color: #0078d4;
  border-radius: 4px 4px 0 0;
  color: #0078d4;
  font-weight: 600;
  background: #fff;
}

.home-table-wrapper {
  border-radius: 0 0 8px 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
}

.home-table {
  margin-bottom: 0;
  background: #fff;
}

.home-table thead th {
  white-space: nowrap;
  border: 1px solid #dbdbdb;
  padding: 0.5rem 0.75rem;
}

.home-table thead th:not(.geo-header) {
  background-color: #fafafa;
}

.home-table td,
.home-table thead th:not(.sticky-col) {
  min-width: 120px;
}

.home-table td {
  border: 1px solid #dbdbdb;
  padding: 0.5rem 0.75rem;
  vertical-align: middle;
  white-space: normal;
  word-break: break-word;
}

.home-table tbody tr:hover td {
  background-color: #f0f7ff;
}

.home-table thead th {
  position: sticky;
  top: 0;
  z-index: 2;
}

.sticky-col {
  position: sticky;
  left: 0;
  z-index: 1;
  background-color: #fff;
  min-width: 180px;
  border-right: 2px solid #dbdbdb !important;
}

.home-table thead .sticky-col {
  z-index: 3;
  background-color: #fafafa;
}

.header-col {
  vertical-align: middle !important;
}

.geo-header {
  font-weight: 600;
  letter-spacing: 0.02em;
}

.region-header {
  font-size: 0.8rem;
  font-weight: 500;
}

.region-link {
  color: var(--azure-blue, #0078d4);
  text-decoration: none;
  transition: color 0.15s;
}

.region-link:hover {
  color: #005a9e;
  text-decoration: underline;
}

.section-row {
  background-color: #f8f9fa !important;
}

.section-row th {
  background-color: #f8f9fa !important;
  font-size: 0.9rem;
  padding-top: 0.8rem;
  padding-bottom: 0.8rem;
}

.home-table td .tag {
  margin: 1px;
}
</style>
