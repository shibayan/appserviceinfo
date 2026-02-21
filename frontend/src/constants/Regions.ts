export interface RegionInfo {
  displayName: string
  address: string
  latitude: number
  longitude: number
  geography: string
}

export interface Geography {
  name: string
  regions: string[]
}

export const regionData: Record<string, RegionInfo> = {
  // Americas
  brazilsouth: { displayName: "Brazil South", address: "São Paulo, Brazil", latitude: -23.5505, longitude: -46.6333, geography: "Americas" },
  canadacentral: { displayName: "Canada Central", address: "Toronto, Ontario, Canada", latitude: 43.6532, longitude: -79.3832, geography: "Americas" },
  centralus: { displayName: "Central US", address: "Iowa, United States", latitude: 41.5908, longitude: -93.6208, geography: "Americas" },
  eastus: { displayName: "East US", address: "Virginia, United States", latitude: 37.3719, longitude: -79.8164, geography: "Americas" },
  eastus2: { displayName: "East US 2", address: "Virginia, United States", latitude: 36.6681, longitude: -78.3889, geography: "Americas" },
  westus3: { displayName: "West US 3", address: "Phoenix, Arizona, United States", latitude: 33.4484, longitude: -112.0740, geography: "Americas" },
  // Europe
  francecentral: { displayName: "France Central", address: "Paris, France", latitude: 48.8566, longitude: 2.3522, geography: "Europe" },
  northeurope: { displayName: "North Europe", address: "Dublin, Ireland", latitude: 53.3478, longitude: -6.2597, geography: "Europe" },
  norwayeast: { displayName: "Norway East", address: "Oslo, Norway", latitude: 59.9139, longitude: 10.7522, geography: "Europe" },
  switzerlandnorth: { displayName: "Switzerland North", address: "Zurich, Switzerland", latitude: 47.4682, longitude: 8.4371, geography: "Europe" },
  uksouth: { displayName: "UK South", address: "London, United Kingdom", latitude: 51.5074, longitude: -0.1278, geography: "Europe" },
  westeurope: { displayName: "West Europe", address: "Amsterdam, Netherlands", latitude: 52.3667, longitude: 4.9000, geography: "Europe" },
  // Middle East
  uaenorth: { displayName: "UAE North", address: "Dubai, United Arab Emirates", latitude: 25.2048, longitude: 55.2708, geography: "Middle East" },
  israelcentral: { displayName: "Israel Central", address: "Tel Aviv, Israel", latitude: 31.2691, longitude: 34.7674, geography: "Middle East" },
  // Asia Pacific
  australiaeast: { displayName: "Australia East", address: "New South Wales, Australia", latitude: -33.8688, longitude: 151.2093, geography: "Asia Pacific" },
  centralindia: { displayName: "Central India", address: "Pune, India", latitude: 18.5204, longitude: 73.8567, geography: "Asia Pacific" },
  eastasia: { displayName: "East Asia", address: "Hong Kong", latitude: 22.3193, longitude: 114.1694, geography: "Asia Pacific" },
  japaneast: { displayName: "Japan East", address: "Tokyo, Japan", latitude: 35.6897, longitude: 139.6922, geography: "Asia Pacific" },
  japanwest: { displayName: "Japan West", address: "Osaka, Japan", latitude: 34.6939, longitude: 135.5022, geography: "Asia Pacific" },
  koreacentral: { displayName: "Korea Central", address: "Seoul, South Korea", latitude: 37.5665, longitude: 126.9780, geography: "Asia Pacific" },
  southeastasia: { displayName: "Southeast Asia", address: "Singapore", latitude: 1.3521, longitude: 103.8198, geography: "Asia Pacific" },
}

export const allRegions = Object.keys(regionData)

const geographyOrder = ["Americas", "Europe", "Middle East", "Asia Pacific"]

export const geographies: Geography[] = geographyOrder.map(name => ({
  name,
  regions: allRegions.filter(r => regionData[r].geography === name)
}))
