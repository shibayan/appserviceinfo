resource "azurerm_static_web_app" "default" {
  name                = "stapp-graffias"
  location            = var.location
  resource_group_name = var.resource_group_name
}

resource "azurerm_static_web_app_custom_domain" "default" {
  static_web_app_id = azurerm_static_web_app.default.id
  domain_name       = var.dns_zone_name
  validation_type   = "dns-txt-token"
}

resource "azurerm_dns_a_record" "default" {
  name                = "@"
  zone_name           = var.dns_zone_name
  resource_group_name = var.resource_group_name
  ttl                 = 3600
  target_resource_id  = azurerm_static_web_app.default.id
}

resource "azurerm_dns_txt_record" "default" {
  name                = "@"
  zone_name           = var.dns_zone_name
  resource_group_name = var.resource_group_name
  ttl                 = 300

  record {
    value = azurerm_static_web_app_custom_domain.default.validation_token
  }
}
