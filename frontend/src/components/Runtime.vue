<script setup lang="ts">
import { ref } from 'vue'
import { Runtime } from '../types/Models'

const props = defineProps<{ region: string }>()

const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${props.region}/runtime.json`)
const json = await response.json()

const runtime = ref<Runtime>(json)
</script>

<template>
  <div class="card">
    <div class="card-header">
      <p class="card-header-title">
        <span class="icon mr-2 has-text-success"><i class="fa-solid fa-code"></i></span>
        Runtime
      </p>
    </div>
    <div class="card-content">
      <!-- .NET Section -->
      <h5 class="runtime-category">
        <span class="icon-text">
          <span class="icon"><i class="fa-brands fa-microsoft"></i></span>
          <span>.NET</span>
        </span>
      </h5>
      <div class="runtime-row">
        <span class="runtime-label">.NET Framework</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnet.allVersions" :key="item.version" class="tag is-link is-light">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">.NET (x86)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnetCore.allVersions" :key="item.version" class="tag is-link is-light">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">.NET (x64)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnetCore64.allVersions" :key="item.version" class="tag is-link is-light">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">.NET SDK (x86)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnetCoreSdk.allVersions" :key="item.version" class="tag is-info is-light">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">.NET SDK (x64)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnetCoreSdk64.allVersions" :key="item.version" class="tag is-info is-light">{{ item.version }}</span>
        </div>
      </div>

      <!-- Java Section -->
      <h5 class="runtime-category mt-5">
        <span class="icon-text">
          <span class="icon"><i class="fa-brands fa-java"></i></span>
          <span>Java</span>
        </span>
      </h5>
      <div class="runtime-row">
        <span class="runtime-label">Java (Oracle)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.oracleJava.allVersions" :key="item.version" class="tag is-danger is-light">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">Java (Azul)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.azulJava.allVersions" :key="item.version" class="tag is-danger is-light">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">Java (Microsoft)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.microsoftJava.allVersions" :key="item.version" class="tag is-danger is-light">{{ item.version }}</span>
        </div>
      </div>

      <!-- Node.js Section -->
      <h5 class="runtime-category mt-5">
        <span class="icon-text">
          <span class="icon"><i class="fa-brands fa-node-js"></i></span>
          <span>Node.js</span>
        </span>
      </h5>
      <div class="runtime-row">
        <span class="runtime-label">Node.js (x86)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.node.allVersions" :key="item.version" class="tag is-success is-light">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">Node.js (x64)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.node64.allVersions" :key="item.version" class="tag is-success is-light">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">NPM</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.npm.allVersions" :key="item.version" class="tag is-success is-light">{{ item.version }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
.card {
  border-radius: 8px;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.06);
  border: 1px solid #eee;
}

.card-header {
  border-bottom: 2px solid #f0f7ff;
}

.runtime-category {
  font-size: 0.9rem;
  font-weight: 700;
  color: #374151;
  padding-bottom: 0.5rem;
  border-bottom: 1px solid #f3f4f6;
  margin-bottom: 0.75rem;
}

.runtime-row {
  display: flex;
  align-items: flex-start;
  padding: 0.5rem 0;
  border-bottom: 1px solid #f9fafb;
}

.runtime-row:last-child {
  border-bottom: none;
}

.runtime-label {
  min-width: 160px;
  font-size: 0.85rem;
  font-weight: 500;
  color: #6b7280;
  padding-top: 0.25rem;
  flex-shrink: 0;
}

.runtime-tags {
  display: flex;
  flex-wrap: wrap;
  gap: 0.25rem;
}
</style>
