<script setup lang="ts">
import { ref } from 'vue'

import Platform from './components/Platform.vue'
import Runtime from './components/Runtime.vue'
import SiteExtension from './components/SiteExtension.vue'

const currentLocation = ref<string>("japaneast")
</script>

<template>
  <Suspense>
    <template #default>
      <Platform :location="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
  <hr />
  <Suspense>
    <template #default>
      <Runtime :location="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
  <hr />
  <Suspense>
    <template #default>
      <SiteExtension :location="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
</template>