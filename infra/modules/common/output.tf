output "resource_group_name" {
  value = azurerm_resource_group.default.name
}

output "dns_zone_name" {
  value = azurerm_dns_zone.default.name
}

output "storage_account_name" {
  value = azurerm_storage_account.default.name
}