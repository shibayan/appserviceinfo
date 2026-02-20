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
  <nav class="breadcrumb" aria-label="breadcrumbs">
    <ul>
      <li><RouterLink to="/"><span class="icon is-small"><i class="fa-solid fa-house"></i></span><span>Home</span></RouterLink></li>
      <li class="is-active"><a href="#" aria-current="page">{{ currentLocation }}</a></li>
    </ul>
  </nav>

  <h2 class="title is-3 mb-5">
    <span class="icon-text">
      <span class="icon has-text-link"><i class="fa-solid fa-location-dot"></i></span>
      <span>{{ currentLocation }}</span>
    </span>
  </h2>

  <div class="columns is-multiline">
    <div class="column is-12">
      <Suspense timeout="250">
        <template #default>
          <Platform :location="currentLocation" :key="currentLocation" />
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
    </div>
    <div class="column is-12">
      <Suspense timeout="250">
        <template #default>
          <Runtime :location="currentLocation" :key="currentLocation" />
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
    </div>
    <div class="column is-12">
      <Suspense timeout="250">
        <template #default>
          <SiteExtension :location="currentLocation" :key="currentLocation" />
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
    </div>
  </div>
</template>
