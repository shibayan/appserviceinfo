export class Geographies {
  private static geographies = [
    { name: "United States", locations: ["westus2", "eastus2", "centralus"] },
    { name: "Asia Pacific", locations: ["eastasia", "southeastasia"] },
    { name: "Europe", locations: ["northeurope", "westeurope"] },
    { name: "Japan", locations: ["japaneast"] },
    { name: "Korea", locations: ["koreacentral"] },
    { name: "Australia", locations: ["australiaeast"] },
    { name: "France", locations: ["francecentral"] },
    { name: "United Kingdom", locations: ["uksouth"] }
  ]

  static getGeographies() {
    return this.geographies;
  }

  static getLocations() {
    return this.geographies.flatMap(x => x.locations);
  }
}