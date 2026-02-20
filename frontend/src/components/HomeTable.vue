<script setup lang="ts">
import { Platform, Runtime, SiteExtension } from '../types/Models'
import { formatRelativeTime } from '../utils/FormatDate'
import { Geographies } from '../constants/Geographies'

const geographies = Geographies.getGeographies();
const locations = Geographies.getLocations();

const platformList = Array<Platform>();

for (const location of locations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/platform.json`)

  if (response.status === 404) {
    platformList.push(new Platform());
  } else {
    platformList.push(await response.json() as Platform)
  }
}

const runtimeList = Array<Runtime>();

for (const location of locations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/runtime.json`)

  if (response.status === 404) {
    runtimeList.push(new Runtime());
  } else {
    runtimeList.push(await response.json() as Runtime);
  }
}

const siteExtensionsList = Array<SiteExtension[]>();

for (const location of locations) {
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
          <th v-for="geography in geographies" :colspan="geography.locations.length"
            class="has-text-centered geo-header" :class="geographyColors[geography.name]">
            {{ geography.name }}
          </th>
        </tr>
        <tr>
          <th v-for="location in locations" class="has-text-centered location-header">
            <RouterLink :to="{ name: 'Location', params: { location: location } }" class="location-link">
              {{ location }}
            </RouterLink>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr class="section-row">
          <th class="sticky-col" :colspan="locations.length + 1">
            <span class="icon-text">
              <span class="icon has-text-link"><i class="fa-solid fa-server"></i></span>
              <span>Platform</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">OS Version</th>
          <td v-for="platform in platformList" class="is-size-7">{{ platform.osVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">App Service Version</th>
          <td v-for="platform in platformList" class="is-size-7">{{ platform.appServiceVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Kudu Version</th>
          <td v-for="platform in platformList" class="is-size-7">{{ platform.kuduVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Middleware Module</th>
          <td v-for="platform in platformList" class="is-size-7">{{ platform.middlewareModuleVersion }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Processor Name</th>
          <td v-for="platform in platformList" class="is-size-7">{{ platform.processorName }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Last Reimage</th>
          <td v-for="platform in platformList" class="is-size-7">{{ formatRelativeTime(platform.lastReimage) }}</td>
        </tr>
        <tr>
          <th class="sticky-col">Last Rapid Update</th>
          <td v-for="platform in platformList" class="is-size-7">{{ formatRelativeTime(platform.lastRapidUpdate) }}
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Stampname</th>
          <td v-for="platform in platformList" class="is-size-7">{{ platform.currentStampname }}</td>
        </tr>

        <tr class="section-row">
          <th class="sticky-col" :colspan="locations.length + 1">
            <span class="icon-text">
              <span class="icon has-text-success"><i class="fa-solid fa-code"></i></span>
              <span>Runtime</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">.NET Framework</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.dotnet.latestVersions" class="tag is-link is-light is-small">{{ item.version
              }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">.NET (x86)</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.dotnetCore.latestVersions" class="tag is-link is-light is-small">{{
              item.version }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">.NET (x64)</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.dotnetCore64.latestVersions" class="tag is-link is-light is-small">{{
              item.version }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Node.js (x86)</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.node.latestVersions" class="tag is-success is-light is-small">{{ item.version
              }}</span>
          </td>
        </tr>
        <tr>
          <th class="sticky-col">Node.js (x64)</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.node64.latestVersions" class="tag is-success is-light is-small">{{
              item.version }}</span>
          </td>
        </tr>

        <tr class="section-row">
          <th class="sticky-col" :colspan="locations.length + 1">
            <span class="icon-text">
              <span class="icon has-text-warning-dark"><i class="fa-solid fa-puzzle-piece"></i></span>
              <span>Site Extensions</span>
            </span>
          </th>
        </tr>
        <tr>
          <th class="sticky-col">Functions</th>
          <td v-for="siteExtensions in siteExtensionsList">
            <span v-for="item in siteExtensions.find(x => x.name === 'Functions')?.installed.latestVersions"
              class="tag is-warning is-light is-small">{{ item.version }}</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<style scoped>
.home-table-wrapper {
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
  overflow-x: auto;
}

.home-table {
  margin-bottom: 0 !important;
  white-space: nowrap;
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
}
</style>
