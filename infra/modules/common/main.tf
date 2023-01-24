resource "azurerm_resource_group" "default" {
  name     = "rg-graffias"
  location = var.location
}

resource "azurerm_dns_zone" "default" {
  name                = "appservice.info"
  resource_group_name = azurerm_resource_group.default.name
}