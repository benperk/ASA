CURL - https://curl.haxx.se/download.html

PSPING - https://docs.microsoft.com/en-us/sysinternals/downloads/psping

Boht TCPPING and NAMERESOLVER are installed by default on an Azure App Service and are accessible via the KUDU/SCM console.  Access KUDU/SCM using a URL similiar to the following: https:// yourAppServiceName .scm.azurewebsites.net and navigate to the Diagnostic Console.


D:\home>nameresolver

Usage:

  nameresolver.exe host           # Lookup 'host' against default server
  
  nameresolver.exe host server    # Lookup 'host' against a particular server

D:\home>tcpping

tcpping.exe - Opens a TCP socket to a target host:port and returns whether the initial handshake was successful and a connection was established
