<script setup lang="ts">
import { ref, computed } from 'vue'

const isMenuActive = ref(false)
const currentYear = computed(() => new Date().getFullYear())
</script>

<template>
  <nav class="navbar is-dark is-fixed-top" role="navigation" aria-label="main navigation">
    <div class="container">
      <div class="navbar-brand">
        <RouterLink class="navbar-item has-text-weight-bold" to="/">
          <span class="icon mr-2"><i class="fa-brands fa-microsoft"></i></span>
          <span>App Service Info</span>
        </RouterLink>
        <a role="button" class="navbar-burger" :class="{ 'is-active': isMenuActive }" aria-label="menu"
          aria-expanded="false" @click="isMenuActive = !isMenuActive">
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
          <span aria-hidden="true"></span>
        </a>
      </div>
      <div class="navbar-menu" :class="{ 'is-active': isMenuActive }">
        <div class="navbar-end">
          <RouterLink class="navbar-item" to="/timeline">
            <span class="icon"><i class="fa-solid fa-timeline"></i></span>
            <span>Timeline</span>
          </RouterLink>
          <a class="navbar-item" href="https://github.com/shibayan/appserviceinfo" target="_blank" rel="noopener">
            <span class="icon"><i class="fa-brands fa-github"></i></span>
            <span>GitHub</span>
          </a>
        </div>
      </div>
    </div>
  </nav>

  <section class="section">
    <div class="container">
      <RouterView v-slot="{ Component }">
        <component :is="Component" />
      </RouterView>
    </div>
  </section>

  <footer class="footer">
    <div class="container">
      <div class="columns">
        <div class="column">
          <p>
            <strong>App Service Info</strong> &mdash; Azure App Service platform and runtime monitoring
          </p>
          <p class="mt-2 has-text-grey">
            &copy; {{ currentYear }} <a href="https://shibayan.jp" class="has-text-grey-dark">@shibayan</a>
          </p>
        </div>
        <div class="column is-narrow">
          <div class="buttons">
            <a class="button is-small is-dark" href="https://github.com/shibayan/appserviceinfo" target="_blank"
              rel="noopener">
              <span class="icon"><i class="fa-brands fa-github"></i></span>
              <span>View on GitHub</span>
            </a>
          </div>
        </div>
      </div>
    </div>
  </footer>
</template>

<style>
:root {
  --azure-blue: #0078d4;
  --azure-dark: #002050;
  --azure-light: #deecf9;
}

body {
  min-height: 100vh;
  display: flex;
  flex-direction: column;
}

#app {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

.section {
  flex: 1;
  padding-top: 4.25rem;
}

.navbar.is-dark {
  background-color: var(--azure-dark);
}

.footer {
  padding: 2rem 1.5rem;
  background-color: #f5f5f5;
}

.tag {
  margin: 0.15em;
}

.card {
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  border: 1px solid #eee;
}

.card-header {
  border-bottom: 2px solid #f0f7ff;
}

.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease;
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}

.loading-spinner {
  display: flex;
  justify-content: center;
  align-items: center;
  padding: 3rem;
}

.loading-spinner::after {
  content: "";
  width: 2.5rem;
  height: 2.5rem;
  border: 3px solid var(--azure-light);
  border-top-color: var(--azure-blue);
  border-radius: 50%;
  animation: spin 0.8s linear infinite;
}

@keyframes spin {
  to {
    transform: rotate(360deg);
  }
}
</style>
