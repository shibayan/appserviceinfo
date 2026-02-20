<script setup lang="ts">
import HomeTable from '../components/HomeTable.vue'
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
</template>
