<script setup lang="ts">
import { ref, computed } from 'vue'
import { Platform, Runtime, SiteExtension } from '../types/Models'
import { formatRelativeTime } from '../utils/FormatDate'
import { Geographies } from '../constants/Geographies'

const geographies = Geographies.getGeographies();
const allLocations = Geographies.getLocations();

const selectedGeography = ref<string | null>(null);

const filteredGeographies = computed(() => {
  if (selectedGeography.value === null) return geographies;
  return geographies.filter(g => g.name === selectedGeography.value);
});

const filteredLocations = computed(() => {
  return filteredGeographies.value.flatMap(g => g.locations);
});

const filteredPlatformList = computed(() => {
  return filteredLocations.value.map(loc => {
    const idx = allLocations.indexOf(loc);
    return platformList[idx];
  });
});

const filteredRuntimeList = computed(() => {
  return filteredLocations.value.map(loc => {
    const idx = allLocations.indexOf(loc);
    return runtimeList[idx];
  });
});

const filteredSiteExtensionsList = computed(() => {
  return filteredLocations.value.map(loc => {
    const idx = allLocations.indexOf(loc);
    return siteExtensionsList[idx];
  });
});

const platformList = Array<Platform>();

for (const location of allLocations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/platform.json`)

  if (response.status === 404) {
    platformList.push(new Platform());
  } else {
    platformList.push(await response.json() as Platform)
  }
}

const runtimeList = Array<Runtime>();

for (const location of allLocations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/runtime.json`)

  if (response.status === 404) {
    runtimeList.push(new Runtime());
  } else {
    runtimeList.push(await response.json() as Runtime);
  }
}

const siteExtensionsList = Array<SiteExtension[]>();

for (const location of allLocations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/site-extension.json`)

  if (response.status === 404) {
    siteExtensionsList.push(Array<SiteExtension>());
  } else {
    siteExtensionsList.push(await response.json() as SiteExtension[])
  }
}

const geographyColors: Record<string, string> = {
  'Americas': 'is-info',
  'Europe': 'is-success',
  'Middle East': 'is-warning',
  'Asia Pacific': 'is-danger'
}
</script>

<template>
  <div class="geography-tabs">
    <div class="tabs is-boxed">
      <ul>
        <li :class="{ 'is-active': selectedGeography === null }">
          <a @click="selectedGeography = null">
            <span class="icon is-small"><i class="fa-solid fa-globe"></i></span>
            <span>All</span>
          </a>
        </li>
        <li v-for="geography in geographies" :key="geography.name"
          :class="{ 'is-active': selectedGeography === geography.name }">
          <a @click="selectedGeography = geography.name">
            <span class="icon is-small"><i class="fa-solid fa-earth-americas" v-if="geography.name === 'Americas'"></i><i class="fa-solid fa-earth-europe" v-else-if="geography.name === 'Europe' || geography.name === 'Middle East'"></i><i class="fa-solid fa-earth-asia" v-else></i></span>
            <span>{{ geography.name }}</span>
            <span class="tag is-rounded is-small ml-1" :class="geographyColors[geography.name]">{{ geography.locations.length }}</span>
          </a>
        </li>
      </ul>
    </div>
  </div>

  <div class="table-container home-table-wrapper">
    <table class="table is-bordered is-hoverable is-fullwidth home-table">
      <thead>
        <tr>
          <th rowspan="2" class="sticky-col header-col">
            <span class="icon-text">
              <span class="icon"><i class="fa-solid fa-globe"></i></span>
              <span>Region</span>
            </span>
          </th>
          <th v-for="geography in filteredGeographies" :key="geography.name" :colspan="geography.locations.length"
            class="has-text-centered geo-header" :class="geographyColors[geography.name]">
            {{ geography.name }}
          </th>
        </tr>
        <tr>
          <th v-for="location in filteredLocations" :key="location" class="has-text-centered location-header">
            <RouterLink :to="{ name: 'Location', params: { location: location } }" class="location-link">
              {{ location }}
            </RouterLink>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr class="section-row">
          <th class="sticky-col" :colspan="filteredLocations.length + 1">
            <span class="icon-text">
              <span class="icon has-text-link"><i class="fa-solid fa-server"></i></span>
              <span>Platform</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">OS Version</th>
          <td v-for="platform in filteredPlatformList" class="is-size-7">{{ platform.osVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">App Service Version</th>
          <td v-for="platform in filteredPlatformList" class="is-size-7">{{ platform.appServiceVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Kudu Version</th>
          <td v-for="platform in filteredPlatformList" class="is-size-7">{{ platform.kuduVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Middleware Module</th>
          <td v-for="platform in filteredPlatformList" class="is-size-7">{{ platform.middlewareModuleVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Processor Name</th>
          <td v-for="platform in filteredPlatformList" class="is-size-7">{{ platform.processorName }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Last Reimage</th>
          <td v-for="platform in filteredPlatformList" class="is-size-7">{{ formatRelativeTime(platform.lastReimage) }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Last Rapid Update</th>
          <td v-for="platform in filteredPlatformList" class="is-size-7">{{ formatRelativeTime(platform.lastRapidUpdate) }}
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Stampname</th>
          <td v-for="platform in filteredPlatformList" class="is-size-7">{{ platform.currentStampname }}</td>
        </tr>

        <tr class="section-row">
          <th class="sticky-col" :colspan="filteredLocations.length + 1">
            <span class="icon-text">
              <span class="icon has-text-success"><i class="fa-solid fa-code"></i></span>
              <span>Runtime</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">.NET Framework</th>
          <td v-for="runtime in filteredRuntimeList">
            <span v-for="item in runtime.dotnet.latestVersions" class="tag is-link is-light is-small">{{ item.version
              }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">.NET (x86)</th>
          <td v-for="runtime in filteredRuntimeList">
            <span v-for="item in runtime.dotnetCore.latestVersions" class="tag is-link is-light is-small">{{
              item.version }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">.NET (x64)</th>
          <td v-for="runtime in filteredRuntimeList">
            <span v-for="item in runtime.dotnetCore64.latestVersions" class="tag is-link is-light is-small">{{
              item.version }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Node.js (x86)</th>
          <td v-for="runtime in filteredRuntimeList">
            <span v-for="item in runtime.node.latestVersions" class="tag is-success is-light is-small">{{ item.version
              }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Node.js (x64)</th>
          <td v-for="runtime in filteredRuntimeList">
            <span v-for="item in runtime.node64.latestVersions" class="tag is-success is-light is-small">{{
              item.version }}</span>
          </td>
        </tr>

        <tr class="section-row">
          <th class="sticky-col" :colspan="filteredLocations.length + 1">
            <span class="icon-text">
              <span class="icon has-text-warning-dark"><i class="fa-solid fa-puzzle-piece"></i></span>
              <span>Site Extensions</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">Functions</th>
          <td v-for="siteExtensions in filteredSiteExtensionsList">
            <span v-for="item in siteExtensions.find(x => x.name === 'Functions')?.installed.latestVersions"
              class="tag is-warning is-light is-small">{{ item.version }}</span>
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

.geography-tabs .tabs {
  margin-bottom: 0;
}

.geography-tabs .tabs ul {
  border-bottom-color: #dbdbdb;
}

.geography-tabs .tabs li a {
  border-bottom-color: #dbdbdb;
  padding: 0.5em 1em;
  font-size: 0.9rem;
}

.geography-tabs .tabs li.is-active a {
  border-bottom-color: #0078d4;
  color: #0078d4;
  font-weight: 600;
}

.home-table-wrapper {
  border-radius: 0 0 8px 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  overflow-x: auto;
}

.home-table {
  margin-bottom: 0 !important;
}

.home-table thead th {
  white-space: nowrap;
}

.home-table td,
.home-table thead th:not(.sticky-col) {
  min-width: 120px;
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
}

.header-col {
  vertical-align: middle !important;
}

.geo-header {
  font-weight: 600;
  letter-spacing: 0.02em;
}

.location-header {
  font-size: 0.8rem;
  font-weight: 500;
}

.location-link {
  color: var(--azure-blue, #0078d4);
  text-decoration: none;
  transition: color 0.15s;
}

.location-link:hover {
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

.home-table tbody tr:hover td {
  background-color: #f0f7ff;
}

.home-table td {
  vertical-align: middle;
  white-space: normal;
  word-break: break-word;
}

.home-table td .tag {
  margin: 1px;
}
</style>
