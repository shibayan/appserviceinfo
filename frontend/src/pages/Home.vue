<script setup lang="ts">

import PlatformTable from '../components/PlatformTable.vue'

</script>

<template>
  <Suspense timeout="250">
    <template #default>
      <PlatformTable />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
</template>