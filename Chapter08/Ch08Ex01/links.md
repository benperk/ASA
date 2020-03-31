## Download Links

+ Download Metapoloitable [here](https://information.rapid7.com/download-metasploitable-2017.html)
+ Microsoft Virtual Machine Converter 3.0 [here](https://www.microsoft.com/en-us/download/confirmation.aspx?id=42497)

## PowerShell Commands for converting VMDK to a VHD
```
Import-Module "C:\Program Files\Microsoft Virtual Machine Converter\MvmcCmdlet.psd1"

ConvertTo-MvmcVirtualHardDisk `
        -SourceLiteralPath "C:\Temp\Metasploitable.vmdk" `
        -DestinationLiteralPath "C:\Temp\Metasploitable.vhd" `
        -VhdType FixedHardDisk -VhdFormat Vhd
```
