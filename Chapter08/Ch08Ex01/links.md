## Download Links

+ Download Metapoloitable [here](https://information.rapid7.com/download-metasploitable-2017.html)
+ Microsoft Virtual Machine Converter 3.0 [here](https://www.microsoft.com/en-us/download/confirmation.aspx?id=42497)
+ Azure Resource Providers [here](https://docs.microsoft.com/en-us/azure/azure-resource-manager/management/azure-services-resource-providers)
+ Creat a free Azure DevOps account [here](https://aka.ms/devops)

## PowerShell Commands for converting VMDK to a VHD
```
Import-Module "C:\Program Files\Microsoft Virtual Machine Converter\MvmcCmdlet.psd1"

ConvertTo-MvmcVirtualHardDisk `
        -SourceLiteralPath "C:\Temp\Metasploitable.vmdk" `
        -DestinationLiteralPath "C:\Temp\Metasploitable.vhd" `
        -VhdType FixedHardDisk -VhdFormat Vhd
```
