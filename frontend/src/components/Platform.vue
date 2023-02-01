<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps<{ location: string }>()

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

const response = await fetch(`https://app-graffias-${props.location}.azurewebsites.net/api/platform`)
const json = await response.json()

const platform = ref<Platform>(json)
</script>

<template>
  <h4 class="title is-4">Platform</h4>

  <div class="content">
    <dl>
      <dt>OS Version</dt>
      <dd>{{ platform.osVersion }}</dd>
      <dt>App Service Version</dt>
      <dd>{{ platform.appServiceVersion }}</dd>
      <dt>Kudu Version</dt>
      <dd>{{ platform.kuduVersion }}</dd>
      <dt>Middleware Module Version</dt>
      <dd>{{ platform.middlewareModuleVersion }}</dd>
      <dt>Processor Name</dt>
      <dd>{{ platform.processorName }}</dd>
      <dt>Last Reimage</dt>
      <dd>{{ new Date(platform.lastReimage).toString() }}</dd>
      <dt>Last Rapid Update</dt>
      <dd>{{ new Date(platform.lastRapidUpdate).toString() }}</dd>
      <dt>Current Stampname</dt>
      <dd>{{ platform.currentStampname }} ({{ platform.regionName }})</dd>
      <dt>Machine Name</dt>
      <dd>{{ platform.machineName }} ({{ platform.machineName.startsWith("RD") ? "Legacy Worker" : "VMSS Worker" }})</dd>
    </dl>
  </div>
</template>