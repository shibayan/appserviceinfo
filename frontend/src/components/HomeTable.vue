<script setup lang="ts">
import { Platform, Runtime, SiteExtension } from '../types/Models'
import { formatRelativeTime } from '../utils/FormatDate'
import { Geographies } from '../constants/Geographies'

const geographies = Geographies.getGeographies();
const locations = Geographies.getLocations();

const platformList = Array<Platform>();

for (const location of locations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/platform.json`)
  const platform = await response.json() as Platform

  platformList.push(platform);
}

const runtimeList = Array<Runtime>();

for (const location of locations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/runtime.json`)
  const runtime = await response.json() as Runtime

  runtimeList.push(runtime);
}

const siteExtensionsList = Array<SiteExtension[]>();

for (const location of locations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/siteextension.json`)
  const siteExtensions = await response.json() as SiteExtension[]

  siteExtensionsList.push(siteExtensions);
}
</script>

<template>
  <div class="table-container">
    <table class="table">
      <thead>
        <tr>
          <th rowspan="2" align="center" style="vertical-align: middle;">Platform</th>
          <th v-for="geography in geographies" :colspan="geography.locations.length" align="center">{{ geography.name }}
          </th>
        </tr>
        <tr>
          <th v-for="location in locations" align="center">
            <RouterLink :to="{ name: 'Location', params: { location: location } }">{{ location }}</RouterLink>
          </th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <th align="center">OS Version</th>
          <td v-for="platform in platformList">{{ platform.osVersion }}</td>
        </tr>
        <tr>
          <th align="center">App Service Version</th>
          <td v-for="platform in platformList">{{ platform.appServiceVersion }}</td>
        </tr>
        <tr>
          <th align="center">Kudu Version</th>
          <td v-for="platform in platformList">{{ platform.kuduVersion }}</td>
        </tr>
        <tr>
          <th align="center">Middleware Module Version</th>
          <td v-for="platform in platformList">{{ platform.middlewareModuleVersion }}</td>
        </tr>
        <tr>
          <th align="center">Processor Name</th>
          <td v-for="platform in platformList">{{ platform.processorName }}</td>
        </tr>
        <tr>
          <th align="center">Last Reimage</th>
          <td v-for="platform in platformList">{{ formatRelativeTime(platform.lastReimage) }}</td>
        </tr>
        <tr>
          <th align="center">Last Rapid Update</th>
          <td v-for="platform in platformList">{{ formatRelativeTime(platform.lastRapidUpdate) }}</td>
        </tr>
        <tr>
          <th align="center">Current Stampname</th>
          <td v-for="platform in platformList">{{ platform.currentStampname }}</td>
        </tr>
        <tr>
          <th align="center">Machine Name</th>
          <td v-for="platform in platformList">{{ platform.machineName }} ({{ platform.machineName.startsWith("RD") ?
            "Legacy Worker" : "VMSS Worker" }})</td>
        </tr>
        <tr>
          <th align="center">Runtime</th>
          <th :colspan="locations.length"></th>
        </tr>
        <tr>
          <th align="center">.NET Framework</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.dotnet.latestVersions" class="tag is-primary">{{ item.version }}</span>
          </td>
        </tr>
        <tr>
          <th align="center">.NET Core (x86)</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.dotnetCore.latestVersions" class="tag is-primary">{{ item.version }}</span>
          </td>
        </tr>
        <tr>
          <th align="center">.NET Core (x64)</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.dotnetCore64.latestVersions" class="tag is-primary">{{ item.version }}</span>
          </td>
        </tr>
        <tr>
          <th align="center">Node.js (x86)</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.node.latestVersions" class="tag is-primary">{{ item.version }}</span>
          </td>
        </tr>
        <tr>
          <th align="center">Node.js (x64)</th>
          <td v-for="runtime in runtimeList">
            <span v-for="item in runtime.node64.latestVersions" class="tag is-primary">{{ item.version }}</span>
          </td>
        </tr>
        <tr>
          <th align="center">Site Extensions</th>
          <th :colspan="locations.length"></th>
        </tr>
        <tr>
          <th align="center">Functions</th>
          <td v-for="siteExtensions in siteExtensionsList">
            <span v-for="item in siteExtensions.find(x => x.name === 'Functions')?.installed.latestVersions"
              class="tag is-primary">{{ item.version }}</span>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>