mkdir ..\Dist

rem *************************** MOMAdapters *********************************

mkdir ..\Dist\MOMAdapters

copy .\MOMAdapters\MOMAdapters\reg\*.bat ..\Dist\MOMAdapters

copy .\MOMAdapters\MOMAdapters\bin\Debug\* ..\Dist\MOMAdapters
copy .\MOMAdapters\MOMAdapters\bin\Release\* ..\Dist\MOMAdapters


rem *************************** OneCService2 ********************************

mkdir ..\Dist\OneCService2
mkdir ..\Dist\OneCService2\scripts

copy .\OneCService2\OneCService2\scripts\* ..\Dist\OneCService2\scripts

copy .\OneCService2\OneCService2\bin\Debug\* ..\Dist\OneCService2
copy .\OneCService2\OneCService2\bin\Release\* ..\Dist\OneCService2
