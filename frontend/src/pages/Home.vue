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
  <div class="flex flex-col">
    <div class="mb-6">
      <div class="title-row mb-2">
        <h1 class="text-[1.72rem] font-extrabold leading-[1.08] tracking-[-0.015em]">
          <span class="inline-flex items-center gap-0.5">
            <span class="inline-flex items-center justify-center w-[1.35rem] h-[1.35rem] text-[1.16rem] text-blue-500"><i class="fa-solid fa-table-columns"></i></span>
            <span>Version Matrix</span>
          </span>
        </h1>
        <span v-if="snapshotUpdatedAt" class="tag tag-md tag-info snapshot-badge">
          Snapshot {{ formatRelativeTime(snapshotUpdatedAt) }}
        </span>
      </div>
      <p class="text-gray-500 text-[0.98rem] leading-[1.45]">
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
  <div v-if="loadError" class="notification notification-warning mt-4">
    <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-solid fa-triangle-exclamation"></i></span>
    Failed to load region data. Please try again later.
  </div>
  </div>
</template>

<style scoped>
.title-row {
  display: flex;
  flex-wrap: wrap;
  align-items: center;
  gap: 0.45rem;
}

.snapshot-badge {
  white-space: nowrap;
  margin-left: 0.15rem;
  padding: 0.35em 0.7em;
  font-size: 0.82rem;
  line-height: 1.2;
}

@media (max-width: 768px) {
  h1 {
    font-size: 1.5rem !important;
  }
}
</style>
