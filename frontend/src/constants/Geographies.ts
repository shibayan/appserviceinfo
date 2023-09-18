export class Geographies {
  private static geographies = [
    { name: "Americas", locations: ["westus2", "eastus2", "centralus", "canadacentral"] },
    { name: "Europe", locations: ["northeurope", "westeurope", "francecentral", "uksouth"] },
    { name: "Middle East", locations: ["qatarcentral"] },
    { name: "Asia Pacific", locations: ["eastasia", "southeastasia", "japaneast", "koreacentral", "australiaeast", "centralindia"] }
  ]

  static getGeographies() {
    return this.geographies;
  }

  static getLocations() {
    return this.geographies.flatMap(x => x.locations);
  }
}