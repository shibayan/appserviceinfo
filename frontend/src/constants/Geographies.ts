export class Geographies {
  private static geographies = [
    { name: "Americas", locations: ["westus2", "westus3", "eastus", "eastus2", "centralus", "southcentralus", "canadacentral"] },
    { name: "Europe", locations: ["northeurope", "westeurope", "francecentral", "uksouth"] },
    { name: "Middle East", locations: ["qatarcentral", "uaenorth"] },
    { name: "Asia Pacific", locations: ["eastasia", "southeastasia", "japaneast", "koreacentral", "australiaeast", "centralindia"] }
  ]

  static getGeographies() {
    return this.geographies;
  }

  static getLocations() {
    return this.geographies.flatMap(x => x.locations);
  }
}