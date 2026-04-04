<script setup lang="ts">
import { computed, ref, onErrorCaptured } from 'vue'
import { useRoute } from 'vue-router'

import Platform from '../components/Platform.vue'
import RegionInfo from '../components/RegionInfo.vue'
import Runtime from '../components/Runtime.vue'
import SiteExtension from '../components/SiteExtension.vue'
import { regionData } from '../constants/Regions'

const route = useRoute()

const currentRegion = computed(() => route.params.region as string)

const regionDisplayName = computed(() => regionData[currentRegion.value]?.displayName || currentRegion.value)

const errors = ref<Record<string, string>>({})

onErrorCaptured((err, instance) => {
  const name = instance?.$options.__name || 'unknown'
  errors.value[name] = err instanceof Error ? err.message : String(err)
  return false
})
</script>

<template>
  <nav class="breadcrumb" aria-label="breadcrumbs">
    <ul>
      <li><RouterLink to="/"><span class="icon is-small"><i class="fa-solid fa-house"></i></span><span>Home</span></RouterLink></li>
      <li class="is-active"><a href="#" aria-current="page">{{ regionDisplayName }}</a></li>
    </ul>
  </nav>

  <h2 class="title is-3 mb-5">
    <span class="icon-text">
      <span class="icon has-text-link"><i class="fa-solid fa-location-dot"></i></span>
      <span>{{ regionDisplayName }}</span>
    </span>
  </h2>

  <div class="columns is-multiline">
    <div class="column is-12">
      <RegionInfo :region="currentRegion" :key="currentRegion" />
    </div>
    <div class="column is-12">
      <Suspense timeout="250">
        <template #default>
          <Platform :region="currentRegion" :key="currentRegion" />
        </template>
        <template #fallback>
          <div class="card">
            <div class="card-header">
              <p class="card-header-title"><span class="icon mr-2"><i class="fa-solid fa-server"></i></span>Platform</p>
            </div>
            <div class="card-content"><div class="loading-spinner"></div></div>
          </div>
        </template>
      </Suspense>
      <div v-if="errors['Platform']" class="notification is-warning is-light mt-2">
        <span class="icon"><i class="fa-solid fa-triangle-exclamation"></i></span>
        Failed to load platform data.
      </div>
    </div>
    <div class="column is-12">
      <Suspense timeout="250">
        <template #default>
          <Runtime :region="currentRegion" :key="currentRegion" />
        </template>
        <template #fallback>
          <div class="card">
            <div class="card-header">
              <p class="card-header-title"><span class="icon mr-2"><i class="fa-solid fa-code"></i></span>Runtime</p>
            </div>
            <div class="card-content"><div class="loading-spinner"></div></div>
          </div>
        </template>
      </Suspense>
      <div v-if="errors['Runtime']" class="notification is-warning is-light mt-2">
        <span class="icon"><i class="fa-solid fa-triangle-exclamation"></i></span>
        Failed to load runtime data.
      </div>
    </div>
    <div class="column is-12">
      <Suspense timeout="250">
        <template #default>
          <SiteExtension :region="currentRegion" :key="currentRegion" />
        </template>
        <template #fallback>
          <div class="card">
            <div class="card-header">
              <p class="card-header-title"><span class="icon mr-2"><i class="fa-solid fa-puzzle-piece"></i></span>Site Extensions</p>
            </div>
            <div class="card-content"><div class="loading-spinner"></div></div>
          </div>
        </template>
      </Suspense>
      <div v-if="errors['SiteExtension']" class="notification is-warning is-light mt-2">
        <span class="icon"><i class="fa-solid fa-triangle-exclamation"></i></span>
        Failed to load site extension data.
      </div>
    </div>
  </div>
</template>
