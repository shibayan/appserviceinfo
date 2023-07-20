resource "azurerm_resource_group" "default" {
  name     = "rg-graffias"
  location = var.location
}

resource "azurerm_dns_zone" "default" {
  name                = "appservice.info"
  resource_group_name = azurerm_resource_group.default.name
}

resource "azurerm_storage_account" "default" {
  name                     = "stgraffias"
  location                 = azurerm_resource_group.default.location
  resource_group_name      = azurerm_resource_group.default.name
  account_tier             = "Standard"
  account_kind             = "StorageV2"
  account_replication_type = "LRS"

  blob_properties {
    cors_rule {
      allowed_methods    = ["GET", "OPTIONS"]
      allowed_origins    = ["https://appservice.info"]
      allowed_headers    = ["*"]
      exposed_headers    = ["*"]
      max_age_in_seconds = 600
    }
  }
}

resource "azurerm_storage_container" "default" {
  name                  = "metadata"
  storage_account_name  = azurerm_storage_account.default.name
  container_access_type = "blob"
}