<script setup lang="ts">
import { ref } from 'vue'
import { Runtime } from '../types/Models'

const props = defineProps<{ region: string }>()

const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${props.region}/runtime.json`)

if (!response.ok) {
  throw new Error(`Failed to load runtime data (${response.status})`)
}

const runtime = ref<Runtime>(await response.json())
</script>

<template>
  <div class="card">
    <div class="card-header">
      <p class="card-header-title">
        <span class="inline-flex items-center justify-center w-5 h-5 mr-2 text-green-600"><i class="fa-solid fa-code"></i></span>
        Runtime
      </p>
    </div>
    <div class="card-content">
      <!-- .NET Section -->
      <h5 class="runtime-category">
        <span class="inline-flex items-center gap-1">
          <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-brands fa-microsoft"></i></span>
          <span>.NET</span>
        </span>
      </h5>
      <div class="runtime-row">
        <span class="runtime-label">.NET Framework</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnet.allVersions" :key="item.version" class="tag tag-link">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">.NET (x86)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnetCore.allVersions" :key="item.version" class="tag tag-link">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">.NET (x64)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnetCore64.allVersions" :key="item.version" class="tag tag-link">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">.NET SDK (x86)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnetCoreSdk.allVersions" :key="item.version" class="tag tag-info">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">.NET SDK (x64)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.dotnetCoreSdk64.allVersions" :key="item.version" class="tag tag-info">{{ item.version }}</span>
        </div>
      </div>

      <!-- Java Section -->
      <h5 class="runtime-category mt-6">
        <span class="inline-flex items-center gap-1">
          <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-brands fa-java"></i></span>
          <span>Java</span>
        </span>
      </h5>
      <div class="runtime-row">
        <span class="runtime-label">Java (Oracle)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.oracleJava.allVersions" :key="item.version" class="tag tag-danger">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">Java (Azul)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.azulJava.allVersions" :key="item.version" class="tag tag-danger">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">Java (Microsoft)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.microsoftJava.allVersions" :key="item.version" class="tag tag-danger">{{ item.version }}</span>
        </div>
      </div>

      <!-- Node.js Section -->
      <h5 class="runtime-category mt-6">
        <span class="inline-flex items-center gap-1">
          <span class="inline-flex items-center justify-center w-5 h-5"><i class="fa-brands fa-node-js"></i></span>
          <span>Node.js</span>
        </span>
      </h5>
      <div class="runtime-row">
        <span class="runtime-label">Node.js (x86)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.node.allVersions" :key="item.version" class="tag tag-success">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">Node.js (x64)</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.node64.allVersions" :key="item.version" class="tag tag-success">{{ item.version }}</span>
        </div>
      </div>
      <div class="runtime-row">
        <span class="runtime-label">NPM</span>
        <div class="runtime-tags">
          <span v-for="item in runtime.npm.allVersions" :key="item.version" class="tag tag-success">{{ item.version }}</span>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
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
