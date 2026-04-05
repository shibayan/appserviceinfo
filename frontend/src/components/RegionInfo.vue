<script setup lang="ts">
import { computed } from 'vue'
import { regionData } from '../constants/Regions'

const props = defineProps<{ region: string }>()

const info = computed(() => regionData[props.region])

const mapSrc = computed(() => {
  if (!info.value) return ''
  const { latitude: lat, longitude: lon } = info.value
  const delta = 0.5
  return `https://www.openstreetmap.org/export/embed.html?bbox=${lon - delta},${lat - delta},${lon + delta},${lat + delta}&layer=mapnik&marker=${lat},${lon}`
})

const mapsLink = computed(() => {
  if (!info.value) return ''
  const { latitude: lat, longitude: lon } = info.value
  return `https://www.openstreetmap.org/?mlat=${lat}&mlon=${lon}#map=10/${lat}/${lon}`
})
</script>

<template>
  <div v-if="info" class="card">
    <div class="card-header">
      <p class="card-header-title">
        <span class="inline-flex items-center justify-center w-5 h-5 mr-2 text-blue-600"><i class="fa-solid fa-map-location-dot"></i></span>
        Location
      </p>
    </div>
    <div class="card-content">
      <div class="flex flex-col md:flex-row gap-6">
        <div class="md:w-1/3">
          <div class="detail-item">
            <span class="detail-label">Region Name</span>
            <span class="detail-value">{{ info.displayName }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Address</span>
            <span class="detail-value">{{ info.address }}</span>
          </div>
          <div class="detail-item">
            <span class="detail-label">Coordinates</span>
            <span class="detail-value">{{ info.latitude }}, {{ info.longitude }}</span>
          </div>
          <div class="detail-item mt-4">
            <a :href="mapsLink" target="_blank" rel="noopener"
              class="inline-flex items-center gap-1.5 rounded bg-gray-100 hover:bg-gray-200 text-gray-700 text-xs px-3 py-1.5 no-underline">
              <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-solid fa-up-right-from-square"></i></span>
              <span>Open in OpenStreetMap</span>
            </a>
          </div>
        </div>
        <div class="md:w-2/3">
          <iframe
            :src="mapSrc"
            class="map-frame"
            title="Region location map"
            loading="lazy"
            sandbox="allow-scripts allow-same-origin"
          ></iframe>
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

.map-frame {
  width: 100%;
  height: 300px;
  border: 1px solid #eee;
  border-radius: 4px;
}
</style>
