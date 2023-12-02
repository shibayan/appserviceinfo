export class Geographies {
  private static geographies = [
    { name: "Americas", locations: ["brazilsouth", "canadacentral", "centralus", "eastus", "eastus2", "southcentralus", "westus2", "westus3"] },
    { name: "Europe", locations: ["francecentral", "northeurope", "uksouth", "westeurope"] },
    { name: "Middle East", locations: ["qatarcentral", "uaenorth"] },
    { name: "Asia Pacific", locations: ["australiaeast", "centralindia", "eastasia", "japaneast", "koreacentral", "southeastasia"] }
  ]

  static getGeographies() {
    return this.geographies;
  }

  static getLocations() {
    return this.geographies.flatMap(x => x.locations);
  }
}