resource "azurerm_static_site" "default" {
  name                = "stapp-graffias"
  location            = var.location
  resource_group_name = var.resource_group_name
}

resource "azurerm_static_site_custom_domain" "default" {
  static_site_id  = azurerm_static_site.default.id
  domain_name     = var.dns_zone_name
  validation_type = "dns-txt-token"
}

resource "azurerm_dns_txt_record" "default" {
  name                = "_dnsauth"
  zone_name           = var.dns_zone_name
  resource_group_name = var.resource_group_name
  ttl                 = 300

  record {
    value = azurerm_static_site_custom_domain.default.validation_token
  }
}