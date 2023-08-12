<script setup lang="ts">

import HomeTable from '../components/HomeTable.vue'

</script>

<template>
  <Suspense timeout="250">
    <template #default>
      <HomeTable />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
</template>