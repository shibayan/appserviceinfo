<script setup lang="ts">
import { ref, onErrorCaptured } from 'vue'
import HomeTable from '../components/HomeTable.vue'
import { formatRelativeTime } from '../utils/FormatDate'

const loadError = ref(false)
const snapshotUpdatedAt = ref('')

onErrorCaptured(() => {
  loadError.value = true
  return false
})
</script>

<template>
  <div class="home-page">
    <div class="home-header mb-5">
      <div class="title-row mb-2">
        <h1 class="title mb-0 home-title">
          <span class="icon-text home-title-text">
            <span class="icon has-text-info home-title-icon"><i class="fa-solid fa-table-columns"></i></span>
            <span>Version Matrix</span>
          </span>
        </h1>
        <span v-if="snapshotUpdatedAt" class="tag is-info is-light is-medium snapshot-badge">
          Snapshot {{ formatRelativeTime(snapshotUpdatedAt) }}
        </span>
      </div>
      <p class="subtitle is-6 mb-0 home-subtitle">
        Compare the latest platform, runtime, and site extension versions across regions at a glance.
      </p>
    </div>

  <Suspense timeout="250">
    <template #default>
      <HomeTable @snapshot-resolved="snapshotUpdatedAt = $event" />
    </template>
    <template #fallback>
      <div class="loading-spinner"></div>
    </template>
  </Suspense>
  <div v-if="loadError" class="notification is-warning is-light mt-4">
    <span class="icon"><i class="fa-solid fa-triangle-exclamation"></i></span>
    Failed to load region data. Please try again later.
  </div>
  </div>
</template>

<style scoped>
.home-page {
  display: flex;
  flex-direction: column;
  gap: 0;
}

.title-row {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 0.45rem;
}

h1.home-title.title {
  font-size: 1.72rem !important;
  line-height: 1.08 !important;
  letter-spacing: -0.015em;
}

.home-title-text {
  display: inline-flex;
  align-items: center;
  gap: 0.12rem;
}

.home-title-icon {
  margin-right: 0 !important;
  width: 1.35rem;
  height: 1.35rem;
  min-width: 1.35rem;
  font-size: 1.16rem !important;
}

.home-subtitle {
  color: #6b7280;
  font-size: 0.98rem;
  line-height: 1.45;
}

.snapshot-badge {
  white-space: nowrap;
  margin-left: 0.15rem;
  padding: 0.35em 0.7em;
  font-size: 0.82rem;
  line-height: 1.2;
}

@media (max-width: 768px) {
  h1.home-title.title {
    font-size: 1.5rem !important;
  }

  .home-title-icon {
    width: 1.18rem;
    height: 1.18rem;
    min-width: 1.18rem;
    font-size: 1.02rem !important;
  }
}
</style>
