strDir = ".\Util\instaler"

Set FSO = CreateObject("Scripting.FileSystemObject")
Set objDir = FSO.GetFolder(strDir)

Set oShell = WScript.CreateObject ("WScript.Shell")

Set objSystemVariables = oShell.Environment("System") 

nsisDir = ""
nsisDir = objSystemVariables("NSIS_HOME")

set subversion = WScript.CreateObject("SubWCRev.object")
reposPath = FSO.GetAbsolutePathName(strDir & "\..\..\")
subversion.GetWCInfo reposPath,1,1

WScript.StdOut.WriteLine "svn status of " & reposPath & "  -  " & subversion.revision

version = "0.1"
revision = "" & subversion.Revision
netVersion = "35"
platform = "win32"

outFile = " /DOutFile=..\..\Dist\OneCService2Setup-ver" & version &"-" & platform & "-net" & netVersion & "-r" & revision & ".exe" & " "
productDefine = " /DProductName=""One C Connectors"" "
productVersionDefine = " /DProductVersion=0.1 "
fullDefine = "" & outFile & "" & productDefine & "" & productVersionDefine
command = """" & nsisDir & "\makensis.exe""" & fullDefine & "" & objDir &"\OneCService2.nsi"
'WScript.StdOut.WriteLine command'
retCode = oShell.Run(command, 1, True)

