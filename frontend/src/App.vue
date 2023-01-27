<script setup lang="ts">
import { ref } from 'vue'

import Platform from './components/Platform.vue'
import Runtime from './components/Runtime.vue'
import SiteExtension from './components/SiteExtension.vue'

const locations = ["japaneast", "westus2", "eastus2"]
const currentLocation = ref<string>("japaneast")
</script>

<template>
  <Suspense timeout="0">
    <template #default>
      <Platform :location="currentLocation" :key="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
  <hr />
  <Suspense timeout="0">
    <template #default>
      <Runtime :location="currentLocation" :key="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
  <hr />
  <Suspense timeout="0">
    <template #default>
      <SiteExtension :location="currentLocation" :key="currentLocation" />
    </template>
    <template #fallback>
      <span>Loading...</span>
    </template>
  </Suspense>
  <hr />
  <div class="field-body">
    <div class="field">
      <div class="control">
        <div class="select">
          <select v-model="currentLocation">
            <option v-for="location in locations" :value="location">{{ location }}</option>
          </select>
        </div>
      </div>
    </div>
  </div>
</template>