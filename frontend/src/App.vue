<script setup lang="ts">
import Platform from './components/Platform.vue'
import Runtime from './components/Runtime.vue'
import SiteExtension from './components/SiteExtension.vue';
</script>

<template>
  <Suspense>
    <Platform />
  </Suspense>
  <hr />
  <Suspense>
    <Runtime />
  </Suspense>
  <hr />
  <Suspense>
    <SiteExtension />
  </Suspense>
</template>