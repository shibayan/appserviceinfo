<script setup lang="ts">
import { ref, computed } from 'vue'

const isMenuActive = ref(false)
const currentYear = computed(() => new Date().getFullYear())
</script>

<template>
  <nav class="fixed top-0 inset-x-0 z-50 bg-azure-dark" role="navigation" aria-label="main navigation">
    <div class="site-container">
      <div class="flex items-center h-[3.25rem]">
        <RouterLink class="flex items-center text-white font-bold no-underline gap-2" to="/">
          <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-brands fa-microsoft"></i></span>
          <span>App Service Info</span>
        </RouterLink>
        <button class="ml-auto md:hidden text-white p-2" @click="isMenuActive = !isMenuActive" aria-label="menu"
          aria-expanded="false">
          <i class="fa-solid" :class="isMenuActive ? 'fa-xmark' : 'fa-bars'"></i>
        </button>
        <div class="hidden md:flex items-center ml-auto gap-1">
          <RouterLink
            class="flex items-center text-white/70 hover:text-white px-3 py-2 gap-1.5 no-underline"
            to="/timeline">
            <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-solid fa-timeline"></i></span>
            <span>Timeline</span>
          </RouterLink>
          <a class="flex items-center text-white/70 hover:text-white px-3 py-2 gap-1.5 no-underline"
            href="https://github.com/shibayan/appserviceinfo" target="_blank" rel="noopener">
            <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-brands fa-github"></i></span>
            <span>GitHub</span>
          </a>
        </div>
      </div>
      <div v-if="isMenuActive" class="md:hidden border-t border-white/10 pb-3">
        <RouterLink
          class="flex items-center text-white/70 hover:text-white px-2 py-2 gap-1.5 no-underline"
          to="/timeline" @click="isMenuActive = false">
          <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-solid fa-timeline"></i></span>
          <span>Timeline</span>
        </RouterLink>
        <a class="flex items-center text-white/70 hover:text-white px-2 py-2 gap-1.5 no-underline"
          href="https://github.com/shibayan/appserviceinfo" target="_blank" rel="noopener">
          <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-brands fa-github"></i></span>
          <span>GitHub</span>
        </a>
      </div>
    </div>
  </nav>

  <section class="flex-1 pt-[4.25rem] pb-12">
    <div class="site-container">
      <RouterView v-slot="{ Component }">
        <component :is="Component" />
      </RouterView>
    </div>
  </section>

  <footer class="bg-gray-100 py-8">
    <div class="site-container">
      <div class="flex flex-wrap items-center justify-between gap-4">
        <div>
          <p>
            <strong>App Service Info</strong> &mdash; Azure App Service platform and runtime monitoring
          </p>
          <p class="mt-2 text-gray-500">
            &copy; {{ currentYear }} <a href="https://shibayan.jp"
              class="text-gray-600 hover:text-gray-800 no-underline">@shibayan</a>
          </p>
        </div>
        <div class="shrink-0">
          <a class="inline-flex items-center gap-1.5 rounded bg-gray-800 hover:bg-gray-700 text-white text-xs px-3 py-1.5 no-underline"
            href="https://github.com/shibayan/appserviceinfo" target="_blank" rel="noopener">
            <span class="inline-flex items-center justify-center w-4 h-4"><i class="fa-brands fa-github"></i></span>
            <span>View on GitHub</span>
          </a>
        </div>
      </div>
    </div>
  </footer>
</template>
