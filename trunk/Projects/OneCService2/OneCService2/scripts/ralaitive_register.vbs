'Sc.exe create OneCService2Lst binpath= D:\work\external\one-s-connectors\trunk\Projects\OneCService2\OneCService2\bin\Debug\OneCService2.exe'

strDir = "..\bin"
runConfiguration = "\Debug" 
Set FSO = CreateObject("Scripting.FileSystemObject")
Set oShell = WScript.CreateObject ("WScript.Shell")

fullPath =  strDir & runConfiguration
WScript.StdOut.WriteLine "ralaitivePath: " & fullPath
Set objDir = FSO.GetFolder(fullPath)
WScript.StdOut.WriteLine "fullPath: " & objDir

Set oShell = WScript.CreateObject ("WScript.Shell")
WScript.StdOut.WriteLine "register service by name and ralaitive bin path"
command = "Sc.exe create OneCService2 binpath= " & objDir & "\OneCService2.exe"
WScript.StdOut.WriteLine "command: " & command
returnCode = oShell.Run(command, 1, True)
WScript.StdOut.WriteLine ""

