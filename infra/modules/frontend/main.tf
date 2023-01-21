resource "azurerm_static_site" "default" {
  name                = "stapp-graffias"
  location            = var.location
  resource_group_name = var.resource_group_name
}