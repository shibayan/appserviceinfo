export interface VersionInfo {
  version: string
}

export interface VersionInfoList {
  latestVersions: VersionInfo[]
  allVersions: VersionInfo[]
}

export interface Platform {
  osVersion: string
  appServiceVersion: string
  kuduVersion: string
  middlewareModuleVersion: string
  processorName: string
  lastReimage: string
  lastRapidUpdate: string
  regionName: string
  currentStampname: string
  machineName: string
}

export interface Runtime {
  dotnet: VersionInfoList
  dotnetCore: VersionInfoList
  dotnetCore64: VersionInfoList
  dotnetCoreSdk: VersionInfoList
  dotnetCoreSdk64: VersionInfoList
  oracleJava: VersionInfoList
  azulJava: VersionInfoList
  microsoftJava: VersionInfoList
  node: VersionInfoList
  node64: VersionInfoList
  npm: VersionInfoList
}

export interface SiteExtension {
  name: string
  installed: VersionInfoList
}