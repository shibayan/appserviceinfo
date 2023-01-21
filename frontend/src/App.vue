<script setup lang="ts">
import { ref } from 'vue'

import Platform from './components/Platform.vue'
import Runtime from './components/Runtime.vue'
import SiteExtension from './components/SiteExtension.vue'

const currentLocation = ref<string>("japaneast")
</script>

<template>
  <Suspense>
    <Platform :location="currentLocation" />
  </Suspense>
  <hr />
  <Suspense>
    <Runtime :location="currentLocation" />
  </Suspense>
  <hr />
  <Suspense>
    <SiteExtension :location="currentLocation" />
  </Suspense>
</template>