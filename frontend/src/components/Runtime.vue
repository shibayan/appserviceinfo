<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps<{ location: string }>()

interface VersionInfo {
  version: string
}

interface Runtime {
  dotnet: VersionInfo[]
  dotnetCore: VersionInfo[]
  dotnetCore64: VersionInfo[]
  dotnetCoreSdk: VersionInfo[]
  dotnetCoreSdk64: VersionInfo[]
  oracleJava: VersionInfo[]
  azulJava: VersionInfo[]
  microsoftJava: VersionInfo[]
  node: VersionInfo[]
  node64: VersionInfo[]
  npm: VersionInfo[]
}

const response = await fetch(`https://app-graffias-${props.location}.azurewebsites.net/api/runtime`)
const json = await response.json()

const runtime = ref<Runtime>(json)
</script>

<template>
  <h4 class="title is-4">Runtime</h4>

  <div class="content">
    <dl>
      <dt>.NET Framework</dt>
      <dd>
        <span v-for="item in runtime.dotnet" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>.NET Core (x86)</dt>
      <dd>
        <span v-for="item in runtime.dotnetCore" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>.NET Core (x64)</dt>
      <dd>
        <span v-for="item in runtime.dotnetCore64" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>.NET Core SDK (x86)</dt>
      <dd>
        <span v-for="item in runtime.dotnetCoreSdk" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>.NET Core SDK (x64)</dt>
      <dd>
        <span v-for="item in runtime.dotnetCoreSdk64" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Java (Oracle)</dt>
      <dd>
        <span v-for="item in runtime.oracleJava" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Java (Azul)</dt>
      <dd>
        <span v-for="item in runtime.azulJava" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Java (Microsoft)</dt>
      <dd>
        <span v-for="item in runtime.microsoftJava" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Node.js (x86)</dt>
      <dd>
        <span v-for="item in runtime.node" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Node.js (x64)</dt>
      <dd>
        <span v-for="item in runtime.node64" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>NPM</dt>
      <dd>
        <span v-for="item in runtime.npm" class="tag is-primary">{{ item.version }}</span>
      </dd>
    </dl>
  </div>
</template>