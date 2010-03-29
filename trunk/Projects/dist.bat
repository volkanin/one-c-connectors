@echo off
set curdir= .\%1

echo current dir is =%curdir%=

mkdir %curdir%\..\Dist

rem *************************** MOMAdapters *********************************

mkdir %curdir%\..\Dist\MOMAdapters

copy %curdir%\.\MOMAdapters\MOMAdapters\reg\*.bat %curdir%\..\Dist\MOMAdapters

copy %curdir%\.\MOMAdapters\MOMAdapters\bin\Debug\* %curdir%\..\Dist\MOMAdapters
copy %curdir%\.\MOMAdapters\MOMAdapters\bin\Release\* %curdir%\..\Dist\MOMAdapters


rem *************************** OneCService2 ********************************

mkdir %curdir%\..\Dist\OneCService2
mkdir %curdir%\..\Dist\OneCService2\scripts

copy %curdir%\.\OneCService2\OneCService2\scripts\* %curdir%\..\Dist\OneCService2\scripts

copy %curdir%\.\OneCService2\OneCService2\bin\Debug\* %curdir%\..\Dist\OneCService2
copy %curdir%\.\OneCService2\OneCService2\bin\Release\* %curdir%\..\Dist\OneCService2
