<script setup lang="ts">
import { ref } from 'vue'

const props = defineProps<{ location: string }>()

interface VersionInfo {
  version: string
}

interface SiteExtension {
  name: string
  versions: VersionInfo[]
}

const response = await fetch(`https://app-graffias-${props.location}.azurewebsites.net/api/siteextension`)
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
          <span v-for="ver in item.versions" class="tag is-primary">{{ ver.version }}</span>
        </dd>
      </template>
    </dl>
  </div>
</template>