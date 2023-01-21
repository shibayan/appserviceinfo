resource "azurerm_service_plan" "default" {
  name                = "plan-graffias-${var.location}"
  location            = var.location
  resource_group_name = var.resource_group_name

  os_type  = "Windows"
  sku_name = "F1"
}

resource "azurerm_windows_web_app" "default" {
  name                = "app-graffias-${var.location}"
  location            = var.location
  resource_group_name = var.resource_group_name

  service_plan_id = azurerm_service_plan.default.id

  client_affinity_enabled = false
  https_only              = true

  site_config {
    always_on               = false
    http2_enabled           = true
    ftps_state              = "Disabled"
    minimum_tls_version     = "1.2"
    scm_minimum_tls_version = "1.2"

    application_stack {
      current_stack  = "dotnet"
      dotnet_version = "v7.0"
    }

    cors {
      allowed_origins = [
        "https://appservice.info"
      ]
    }
  }
}