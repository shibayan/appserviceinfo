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
  <nav aria-label="breadcrumbs" class="mb-4">
    <ol class="flex items-center gap-2">
      <li>
        <RouterLink to="/" class="text-azure hover:underline inline-flex items-center gap-1">
          <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-solid fa-house"></i></span>
          <span>Home</span>
        </RouterLink>
      </li>
      <li class="text-gray-400">/</li>
      <li class="text-gray-600">{{ regionDisplayName }}</li>
    </ol>
  </nav>

  <h2 class="text-[2rem] font-extrabold leading-[1.125] mb-6">
    <span class="inline-flex items-center gap-1">
      <span class="inline-flex items-center justify-center w-6 h-6 text-blue-600"><i class="fa-solid fa-location-dot"></i></span>
      <span>{{ regionDisplayName }}</span>
    </span>
  </h2>

  <div class="flex flex-col gap-6">
    <div>
      <RegionInfo :region="currentRegion" :key="currentRegion" />
    </div>
    <div>
      <Suspense timeout="250">
        <template #default>
          <Platform :region="currentRegion" :key="currentRegion" />
        </template>
        <template #fallback>
          <div class="card">
            <div class="card-header">
              <p class="card-header-title"><span class="inline-flex items-center justify-center w-5 h-5 mr-2"><i class="fa-solid fa-server"></i></span>Platform</p>
            </div>
            <div class="card-content"><div class="loading-spinner"></div></div>
          </div>
        </template>
      </Suspense>
      <div v-if="errors['Platform']" class="notification notification-warning mt-2">
        <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-solid fa-triangle-exclamation"></i></span>
        Failed to load platform data.
      </div>
    </div>
    <div>
      <Suspense timeout="250">
        <template #default>
          <Runtime :region="currentRegion" :key="currentRegion" />
        </template>
        <template #fallback>
          <div class="card">
            <div class="card-header">
              <p class="card-header-title"><span class="inline-flex items-center justify-center w-5 h-5 mr-2"><i class="fa-solid fa-code"></i></span>Runtime</p>
            </div>
            <div class="card-content"><div class="loading-spinner"></div></div>
          </div>
        </template>
      </Suspense>
      <div v-if="errors['Runtime']" class="notification notification-warning mt-2">
        <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-solid fa-triangle-exclamation"></i></span>
        Failed to load runtime data.
      </div>
    </div>
    <div>
      <Suspense timeout="250">
        <template #default>
          <SiteExtension :region="currentRegion" :key="currentRegion" />
        </template>
        <template #fallback>
          <div class="card">
            <div class="card-header">
              <p class="card-header-title"><span class="inline-flex items-center justify-center w-5 h-5 mr-2"><i class="fa-solid fa-puzzle-piece"></i></span>Site Extensions</p>
            </div>
            <div class="card-content"><div class="loading-spinner"></div></div>
          </div>
        </template>
      </Suspense>
      <div v-if="errors['SiteExtension']" class="notification notification-warning mt-2">
        <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-solid fa-triangle-exclamation"></i></span>
        Failed to load site extension data.
      </div>
    </div>
  </div>
</template>
