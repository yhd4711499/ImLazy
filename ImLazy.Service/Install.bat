%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\installutil.exe ImLazy.Service.exe
Net Start ImLazyService
sc config ImLazyService start= auto
::pause