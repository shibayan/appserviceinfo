<script setup lang="ts">
import { ref } from 'vue'
import { SiteExtension } from '../types/Models'

const props = defineProps<{ location: string }>()

const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${props.location}/site-extension.json`)
const json = await response.json()

const siteExtensions = ref<SiteExtension[]>(json)
</script>

<template>
  <div class="card">
    <div class="card-header">
      <p class="card-header-title">
        <span class="icon mr-2 has-text-warning-dark"><i class="fa-solid fa-puzzle-piece"></i></span>
        Site Extensions
      </p>
    </div>
    <div class="card-content">
      <div v-for="item in siteExtensions" class="extension-row">
        <span class="extension-name">{{ item.name }}</span>
        <div class="extension-tags">
          <span v-for="ver in item.installed.allVersions" class="tag is-warning is-light">{{ ver.version }}</span>
        </div>
      </div>
      <p v-if="siteExtensions.length === 0" class="has-text-grey-light has-text-centered">
        No site extensions installed
      </p>
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
