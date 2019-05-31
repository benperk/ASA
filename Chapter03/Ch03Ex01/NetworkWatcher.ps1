Install-Module -Name Az -AllowClobber -Force #(as administrator)
Connect-AzAccount
$subscription = Get-AzSubscription -SubscriptionId "########-####-####-####-############"
Set-AzContext $subscription
New-AzResourceGroup -Name CSHARPGUITAR-RG -Location centralus
New-AzNetworkWatcher -Name NetworkWatcherCentralUS -ResourceGroupName CSHARPGUITAR-RG -Location centralus
Get-AzNetworkWatcherReachabilityReport `
  -NetworkWatcherName NetworkWatcherCentralUS `
  -ResourceGroupName CSHARPGUITAR-RG `
  -Location "Central US" `
  -Country "United States" `
  -State "minnesota" `
  -StartTime "2019-05-25" `
  -EndTime "2019-05-30"
Get-AzNetworkWatcherReachabilityReport `
  -NetworkWatcherName NetworkWatcherCentralUS `
  -ResourceGroupName CSHARPGUITAR-RG `
  -Location "Central US" `
  -Country "United States" `
  -State "wisconsin" `
  -StartTime "2019-05-25" `
  -EndTime "2019-05-30"
Get-AzNetworkWatcherReachabilityReport `
  -NetworkWatcherName NetworkWatcherCentralUS `
  -ResourceGroupName CSHARPGUITAR-RG `
  -Location "Central US" `
  -Country "United States" `
  -State "illinois" `
  -StartTime "2019-05-25" `
  -EndTime "2019-05-30"
  Get-AzNetworkWatcherReachabilityReport `
  -NetworkWatcherName NetworkWatcherCentralUS `
  -ResourceGroupName CSHARPGUITAR-RG `
  -Location "Central US" `
  -Country "United States" `
  -State "missouri" `
  -StartTime "2019-05-25" `
  -EndTime "2019-05-30"
Get-AzNetworkWatcherReachabilityReport `
  -NetworkWatcherName NetworkWatcherCentralUS `
  -ResourceGroupName CSHARPGUITAR-RG `
  -Location "Central US" `
  -Country "Germany" `
  -State "bavaria" `
  -StartTime "2019-05-25" `
  -EndTime "2019-05-30"
Get-AzNetworkWatcherReachabilityReport `
  -NetworkWatcherName NetworkWatcherCentralUS `
  -ResourceGroupName CSHARPGUITAR-RG `
  -Location "Central US" `
  -Country "Japan" `
  -State "osaka" `
  -StartTime "2019-05-25" `
  -EndTime "2019-05-30"
Get-AzNetworkWatcherReachabilityReport `
  -NetworkWatcherName NetworkWatcherCentralUS `
  -ResourceGroupName CSHARPGUITAR-RG `
  -Location "Germany Central" `
  -Country "Germany" `
  -State "bavaria" `
  -StartTime "2019-05-25" `
  -EndTime "2019-05-30"

Get-AzNetworkWatcherReachabilityProvidersList -NetworkWatcherName NetworkWatcherCentralUS -ResourceGroupName CSHARPGUITAR-RG
