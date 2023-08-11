<script setup lang="ts">

interface Platform {
  osVersion: string
  appServiceVersion: string
  kuduVersion: string
  middlewareModuleVersion: string
  processorName: string
  lastReimage: string
  lastRapidUpdate: string
  regionName: string
  currentStampname: string
  machineName: string
}

const geographies = [
  { name: "Japan", locations: ["japaneast"] },
  { name: "United States", locations: ["westus2", "eastus2", "centralus"] },
  { name: "Asia Pacific", locations: ["eastasia", "southeastasia"] },
  { name: "Europe", locations: ["northeurope", "westeurope"] },
  { name: "Korea", locations: ["koreacentral"] }
]

const locations = geographies.flatMap(x => x.locations);

const platformList = Array<Platform>();

for (const location of locations) {
  const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${location}/platform.json`)
  const platform = await response.json() as Platform

  platformList.push(platform);
}
</script>

<template>
  <div class="table-container">
    <table class="table">
      <thead>
        <tr>
          <th rowspan="2"></th>
          <th v-for="geography in geographies" :colspan="geography.locations.length" align="center">{{ geography.name }}</th>
        </tr>
        <tr>
          <th v-for="location in locations" align="center">{{ location }}</th>
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
          <td v-for="platform in platformList">{{ new Date(platform.lastReimage).toString() }}</td>
        </tr>
        <tr>
          <th align="center">Last Rapid Update</th>
          <td v-for="platform in platformList">{{ new Date(platform.lastRapidUpdate).toString() }}</td>
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
      </tbody>
    </table>
  </div>
</template>