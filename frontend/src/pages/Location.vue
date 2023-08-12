<script setup lang="ts">
import { ref, watch } from 'vue'
import { useRoute } from 'vue-router';

import Platform from '../components/Platform.vue'
import Runtime from '../components/Runtime.vue'
import SiteExtension from '../components/SiteExtension.vue'

const route = useRoute();

const currentLocation = ref<string>(route.params.location as string)

watch(route, to => currentLocation.value = to.params.location as string)
</script>

<template>
  <Suspense timeout="250">
    <template #default>
      <Platform :location="currentLocation" :key="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
  <hr />
  <Suspense timeout="250">
    <template #default>
      <Runtime :location="currentLocation" :key="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
  <hr />
  <Suspense timeout="250">
    <template #default>
      <SiteExtension :location="currentLocation" :key="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
</template>