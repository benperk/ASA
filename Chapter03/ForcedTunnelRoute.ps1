New-AzRouteTable –Name "CSHARPGUITAR-DB3-ROUTE-TABLE" -ResourceGroupName "CSHARPGUITAR-DB3-RG" –Location "North Europe"
$rt = Get-AzRouteTable –Name "CSHARPGUITAR-DB3-ROUTE-TABLE" -ResourceGroupName "CSHARPGUITAR-DB3-RG" 
Add-AzRouteConfig -Name "ForcedTunnelRoute" -AddressPrefix "0.0.0.0/0" -NextHopType VirtualNetworkGateway -RouteTable $rt
Set-AzRouteTable -RouteTable $rt
$vnet = Get-AzVirtualNetwork -Name "CSHARPGUITAR-VNET-A" -ResourceGroupName "CSHARPGUITAR-DB3-RG"
Set-AzVirtualNetworkSubnetConfig -Name "csharp" -VirtualNetwork $vnet -AddressPrefix "10.0.0.0/24" -RouteTable $rt
Set-AzVirtualNetwork -VirtualNetwork $vnet