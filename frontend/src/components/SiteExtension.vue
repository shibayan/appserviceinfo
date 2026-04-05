<script setup lang="ts">
import { ref } from 'vue'
import { SiteExtension } from '../types/Models'

const props = defineProps<{ region: string }>()

const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${props.region}/site-extension.json`)

if (!response.ok) {
  throw new Error(`Failed to load site extension data (${response.status})`)
}

const siteExtensions = ref<SiteExtension[]>(await response.json())
</script>

<template>
  <div class="card">
    <div class="card-header">
      <p class="card-header-title">
        <span class="inline-flex items-center justify-center w-5 h-5 mr-2 text-amber-700"><i class="fa-solid fa-puzzle-piece"></i></span>
        Site Extensions
      </p>
    </div>
    <div class="card-content">
      <div v-for="item in siteExtensions" :key="item.name" class="extension-row">
        <span class="extension-name">{{ item.name }}</span>
        <div class="extension-tags">
          <span v-for="ver in item.installed.allVersions" :key="ver.version" class="tag tag-warning">{{ ver.version }}</span>
        </div>
      </div>
      <p v-if="siteExtensions.length === 0" class="text-gray-400 text-center">
        No site extensions installed
      </p>
    </div>
  </div>
</template>

<style scoped>
.extension-row {
  display: flex;
  align-items: flex-start;
  padding: 0.6rem 0;
  border-bottom: 1px solid #f3f4f6;
}

.extension-row:last-child {
  border-bottom: none;
}

.extension-name {
  min-width: 200px;
  font-weight: 500;
  font-size: 0.9rem;
  color: #374151;
  padding-top: 0.2rem;
  flex-shrink: 0;
}

.extension-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.25rem;
}
</style>
