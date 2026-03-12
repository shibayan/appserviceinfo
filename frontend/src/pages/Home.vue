<script setup lang="ts">
import { ref, onErrorCaptured } from 'vue'
import HomeTable from '../components/HomeTable.vue'

const loadError = ref(false)

onErrorCaptured(() => {
  loadError.value = true
  return false
})
</script>

<template>
  <Suspense timeout="250">
    <template #default>
      <HomeTable />
    </template>
    <template #fallback>
      <div class="loading-spinner"></div>
    </template>
  </Suspense>
  <div v-if="loadError" class="notification is-warning is-light mt-4">
    <span class="icon"><i class="fa-solid fa-triangle-exclamation"></i></span>
    Failed to load region data. Please try again later.
  </div>
</template>
