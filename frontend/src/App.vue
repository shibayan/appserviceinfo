<script setup lang="ts">
import { ref } from 'vue'

const isMenuActive = ref(false)
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
          <a class="navbar-item" href="https://github.com/shibayan/appserviceinfo" target="_blank" rel="noopener">
            <span class="icon"><i class="fa-brands fa-github"></i></span>
            <span>GitHub</span>
          </a>
        </div>
      </div>
    </div>
  </nav>

  <section class="hero is-link is-small">
    <div class="hero-body">
      <div class="container">
        <p class="title is-4">
          <span class="icon-text">
            <span class="icon"><i class="fa-solid fa-cloud"></i></span>
            <span>Azure App Service Platform &amp; Runtime Information</span>
          </span>
        </p>
        <p class="subtitle is-6">Real-time platform details, runtime versions, and site extension information across
          all Azure regions</p>
      </div>
    </div>
  </section>

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
            &copy; 2025 <a href="https://shibayan.jp" class="has-text-grey-dark">@shibayan</a>
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
}

.navbar.is-dark {
  background-color: var(--azure-dark);
}

.hero.is-link {
  background: linear-gradient(135deg, var(--azure-blue), #005a9e);
}

.footer {
  padding: 2rem 1.5rem;
  background-color: #f5f5f5;
}

.tag {
  margin: 0.15em;
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
