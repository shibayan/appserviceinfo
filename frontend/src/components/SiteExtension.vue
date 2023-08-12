<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps<{ location: string }>()

interface VersionInfo {
  version: string
}

interface VersionInfoList {
  latestVersions: VersionInfo[]
  allVersions: VersionInfo[]
}

interface SiteExtension {
  name: string
  installed: VersionInfoList
}

const response = await fetch(`https://stgraffias.blob.core.windows.net/metadata/${props.location}/siteextension.json`)
const json = await response.json()

const siteExtensions = ref<SiteExtension[]>(json)
</script>

<template>
  <h4 class="title is-4">Site Extensions</h4>

  <div class="content">
    <dl>
      <template v-for="item in siteExtensions">
        <dt>{{ item.name }}</dt>
        <dd>
          <span v-for="ver in item.installed.allVersions" class="tag is-primary">{{ ver.version }}</span>
        </dd>
      </template>
    </dl>
  </div>
</template>