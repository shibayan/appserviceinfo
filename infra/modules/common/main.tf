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
    delete_retention_policy {
      days = 7
    }

    container_delete_retention_policy {
      days    = 7
    }

    cors_rule {
      allowed_methods    = ["GET", "OPTIONS"]
      allowed_origins    = ["https://appservice.info", "http://localhost:5173"]
      allowed_headers    = ["*"]
      exposed_headers    = ["*"]
      max_age_in_seconds = 600
    }
  }
}

resource "azurerm_storage_container" "default" {
  name                  = "metadata"
  storage_account_id  = azurerm_storage_account.default.id
  container_access_type = "blob"
}