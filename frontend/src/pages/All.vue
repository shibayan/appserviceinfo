<script setup lang="ts">
import { ref, toHandlerKey } from 'vue'

import Platform from '../components/Platform.vue'

const geographies = [
  { name: "Japan", locations: ["japaneast"] },
  { name: "United States", locations: ["westus2", "eastus2", "centralus"] },
  { name: "Asia Pacific", locations: ["eastasia", "southeastasia"] },
  { name: "Europe", locations: ["northeurope", "westeurope"] }
]
</script>

<template>
  <table class="table">
    <thead>
      <tr>
        <th></th>
      </tr>
    </thead>
  </table>
  <div v-for="geography in geographies">
    <h3 class="title is-3">{{ geography.name }}</h3>
    <Suspense v-for="location in geography.locations" timeout="300">
      <template #default>
        <Platform :location="location" />
      </template>
      <template #fallback>
        <span>Loading...</span>
      </template>
    </Suspense>
    <hr />
  </div>
</template>