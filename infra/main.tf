terraform {
  required_version = "~> 1.0"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 4.0"
    }
  }
}

provider "azurerm" {
  resource_provider_registrations = "none"

  features {}
}

locals {
  common_location = "japaneast"

  backend_locations = [
    "brazilsouth", "canadacentral", "centralus", "eastus", "eastus2", "southcentralus", "westus2", "westus3",
    "francecentral", "northeurope", "norwayeast", "switzerlandnorth", "uksouth", "westeurope",
    "qatarcentral", "uaenorth", "israelcentral",
    "australiaeast", "centralindia", "eastasia", "japaneast", "japanwest", "koreacentral", "southeastasia"
  ]

  frontend_location = "eastasia"
}

module "common" {
  source = "./modules/common"

  location = local.common_location
}

module "backend" {
  source = "./modules/backend"

  for_each = toset(local.backend_locations)

  location            = each.value
  resource_group_name = module.common.resource_group_name
}

module "frontend" {
  source = "./modules/frontend"

  location            = local.frontend_location
  resource_group_name = module.common.resource_group_name
  dns_zone_name       = module.common.dns_zone_name
}
