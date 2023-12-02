export class VersionInfo {
  version = ""
}

export class VersionInfoList {
  latestVersions = Array<VersionInfo>()
  allVersions = Array<VersionInfo>()
}

export class Platform {
  osVersion = ""
  appServiceVersion = ""
  kuduVersion = ""
  middlewareModuleVersion = ""
  processorName = ""
  lastReimage = ""
  lastRapidUpdate = ""
  regionName = ""
  currentStampname = ""
  machineName = ""
}

export class Runtime {
  dotnet = new VersionInfoList()
  dotnetCore = new VersionInfoList()
  dotnetCore64 = new VersionInfoList()
  dotnetCoreSdk = new VersionInfoList()
  dotnetCoreSdk64 = new VersionInfoList()
  oracleJava = new VersionInfoList()
  azulJava = new VersionInfoList()
  microsoftJava = new VersionInfoList()
  node = new VersionInfoList()
  node64 = new VersionInfoList()
  npm = new VersionInfoList()
}

export class SiteExtension {
  name = ""
  installed = new VersionInfoList()
}