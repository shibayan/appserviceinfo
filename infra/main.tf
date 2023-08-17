provider "azurerm" {
  features {}
}

terraform {
  required_providers {
    azurerm = "~> 3.0"
  }
}

locals {
  common_location = "japaneast"

  backend_locations = [
    "japaneast", "westus2", "eastus2", "centralus", "eastasia",
    "southeastasia", "northeurope", "westeurope", "koreacentral", "australiaeast",
    "francecentral", "uksouth"
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