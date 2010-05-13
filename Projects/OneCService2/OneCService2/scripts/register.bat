SET OCS2_HOME=C:\Work\OneCConnectors\Dist\OneCService2
Sc.exe create OneCService2 binpath= %OCS2_HOME%\OneCService2.exe obj= LocalSystem
