<script setup lang="ts">
import { ref } from 'vue'
import { Runtime } from '../types/Models'

const props = defineProps<{ location: string }>()

const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${props.location}/runtime.json`)
const json = await response.json()

const runtime = ref<Runtime>(json)
</script>

<template>
  <h4 class="title is-4">Runtime</h4>

  <div class="content">
    <dl>
      <dt>.NET Framework</dt>
      <dd>
        <span v-for="item in runtime.dotnet.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>.NET Core (x86)</dt>
      <dd>
        <span v-for="item in runtime.dotnetCore.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>.NET Core (x64)</dt>
      <dd>
        <span v-for="item in runtime.dotnetCore64.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>.NET Core SDK (x86)</dt>
      <dd>
        <span v-for="item in runtime.dotnetCoreSdk.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>.NET Core SDK (x64)</dt>
      <dd>
        <span v-for="item in runtime.dotnetCoreSdk64.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Java (Oracle)</dt>
      <dd>
        <span v-for="item in runtime.oracleJava.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Java (Azul)</dt>
      <dd>
        <span v-for="item in runtime.azulJava.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Java (Microsoft)</dt>
      <dd>
        <span v-for="item in runtime.microsoftJava.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Node.js (x86)</dt>
      <dd>
        <span v-for="item in runtime.node.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>Node.js (x64)</dt>
      <dd>
        <span v-for="item in runtime.node64.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
      <dt>NPM</dt>
      <dd>
        <span v-for="item in runtime.npm.allVersions" class="tag is-primary">{{ item.version }}</span>
      </dd>
    </dl>
  </div>
</template>