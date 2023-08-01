<script setup lang="ts">
import { ref } from 'vue'

import Platform from '../components/Platform.vue'
import Runtime from '../components/Runtime.vue'
import SiteExtension from '../components/SiteExtension.vue'

const geographies = [
  { name: "Japan", locations: ["japaneast"] },
  { name: "United States", locations: ["westus2", "eastus2", "centralus"] },
  { name: "Asia Pacific", locations: ["eastasia", "southeastasia"] },
  { name: "Europe", locations: ["northeurope", "westeurope"] },
  { name: "Korea", locations: ["koreacentral"] }
]

const currentLocation = ref<string>("japaneast")
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
  <hr />
  <div class="field-body">
    <div class="field">
      <div class="control">
        <div class="select">
          <select v-model="currentLocation">
            <optgroup v-for="geography in geographies" :label="geography.name">
              <option v-for="location in geography.locations" :value="location">{{ location }}</option>
            </optgroup>
          </select>
        </div>
      </div>
    </div>
  </div>
</template>