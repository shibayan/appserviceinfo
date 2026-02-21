<script setup lang="ts">
import { ref } from 'vue'
import { Platform } from '../types/Models'
import { formatRelativeTime } from '../utils/FormatDate'
import { locationData } from '../constants/Locations'

const props = defineProps<{ location: string }>()

const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${props.location}/platform.json`)
const json = await response.json()

const platform = ref<Platform>(json)
</script>

<template>
  <div class="card">
    <div class="card-header">
      <p class="card-header-title">
        <span class="icon mr-2 has-text-link"><i class="fa-solid fa-server"></i></span>
        Platform
      </p>
    </div>
    <div class="card-content">
      <div class="columns is-multiline">
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">OS Version</span>
            <span class="detail-value">{{ platform.osVersion }}</span>
          </div>
        </div>
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">App Service Version</span>
            <span class="detail-value">{{ platform.appServiceVersion }}</span>
          </div>
        </div>
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">Kudu Version</span>
            <span class="detail-value">{{ platform.kuduVersion }}</span>
          </div>
        </div>
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">Middleware Module Version</span>
            <span class="detail-value">{{ platform.middlewareModuleVersion }}</span>
          </div>
        </div>
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">Processor Name</span>
            <span class="detail-value">{{ platform.processorName }}</span>
          </div>
        </div>
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">Current Stampname</span>
            <span class="detail-value">{{ platform.currentStampname }} <span class="tag is-light is-small">{{ locationData[props.location]?.displayName || props.location }}</span></span>
          </div>
        </div>
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">Last Reimage</span>
            <span class="detail-value">{{ formatRelativeTime(platform.lastReimage) }}</span>
          </div>
        </div>
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">Last Rapid Update</span>
            <span class="detail-value">{{ formatRelativeTime(platform.lastRapidUpdate) }}</span>
          </div>
        </div>
        <div class="column is-6">
          <div class="detail-item">
            <span class="detail-label">Machine Name</span>
            <span class="detail-value"><code>{{ platform.machineName.toUpperCase() }}</code></span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.card {
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  border: 1px solid #eee;
}

.card-header {
  border-bottom: 2px solid #f0f7ff;
}

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
