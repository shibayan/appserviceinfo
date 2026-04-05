<script setup lang="ts">
import { ref } from 'vue'
import { Platform } from '../types/Models'
import { formatRelativeTime } from '../utils/FormatDate'
import { regionData } from '../constants/Regions'

const props = defineProps<{ region: string }>()

const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${props.region}/platform.json`)

if (!response.ok) {
  throw new Error(`Failed to load platform data (${response.status})`)
}

const platform = ref<Platform>(await response.json())
</script>

<template>
  <div class="card">
    <div class="card-header">
      <p class="card-header-title">
        <span class="inline-flex items-center justify-center w-5 h-5 mr-2 text-blue-600"><i class="fa-solid fa-server"></i></span>
        Platform
      </p>
    </div>
    <div class="card-content">
      <div class="grid grid-cols-1 md:grid-cols-2 gap-x-6">
        <div class="detail-item">
          <span class="detail-label">OS Version</span>
          <span class="detail-value">{{ platform.osVersion }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-label">App Service Version</span>
          <span class="detail-value">{{ platform.appServiceVersion }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-label">Kudu Version</span>
          <span class="detail-value">{{ platform.kuduVersion }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-label">Middleware Module Version</span>
          <span class="detail-value">{{ platform.middlewareModuleVersion }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-label">Processor Name</span>
          <span class="detail-value">{{ platform.processorName }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-label">Current Stampname</span>
          <span class="detail-value">{{ platform.currentStampname }} <span class="tag tag-light tag-sm">{{ regionData[props.region]?.displayName || props.region }}</span></span>
        </div>
        <div class="detail-item">
          <span class="detail-label">Last Reimage</span>
          <span class="detail-value">{{ formatRelativeTime(platform.lastReimage) }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-label">Last Rapid Update</span>
          <span class="detail-value">{{ formatRelativeTime(platform.lastRapidUpdate) }}</span>
        </div>
        <div class="detail-item">
          <span class="detail-label">Machine Name</span>
          <span class="detail-value"><code>{{ platform.machineName.toUpperCase() }}</code></span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.detail-item {
  padding: 0.5rem 0;
}

.detail-label {
  display: block;
  font-size: 0.75rem;
  font-weight: 600;
  color: #6b7280;
  text-transform: uppercase;
  letter-spacing: 0.03em;
  margin-bottom: 0.25rem;
}

.detail-value {
  display: block;
  font-size: 0.95rem;
  color: #1f2937;
  word-break: break-word;
}
</style>
