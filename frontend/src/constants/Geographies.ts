export class Geographies {
  private static geographies = [
    { name: "Americas", locations: ["brazilsouth", "canadacentral", "centralus", "eastus", "eastus2", "westus3"] },
    { name: "Europe", locations: ["francecentral", "northeurope", "norwayeast", "switzerlandnorth", "uksouth", "westeurope"] },
    { name: "Middle East", locations: ["uaenorth", "israelcentral"] },
    { name: "Asia Pacific", locations: ["australiaeast", "centralindia", "eastasia", "japaneast", "japanwest", "koreacentral", "southeastasia"] }
  ]

  static getGeographies() {
    return this.geographies;
  }

  static getLocations() {
    return this.geographies.flatMap(x => x.locations);
  }
}
