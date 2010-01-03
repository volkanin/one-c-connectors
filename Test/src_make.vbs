'/*  source make for all epfs in test dir
'*  
'-------------*/

strDir = ".\"
Set FSO = CreateObject("Scripting.FileSystemObject")
Set objDir = FSO.GetFolder(strDir)
getInfo(objDir)

Function getInfo(pCurrentDir)
   curPath = pCurrentDir.Path
   For Each aItem In pCurrentDir.Files
      If LCase(Right(Cstr(aItem.Name), 4)) = ".epf" Then
          f = runSrcMake(aItem.Name, curPath) 
       End If
    Next

    For Each aItem In pCurrentDir.SubFolders
      If LCase(Cstr(aItem.name)) = ".svn" Then
       	' '
      Else
	    getInfo(aItem)
      End if
Next

End Function

Function runSrcMake(filePath, curPath)
	'wscript.Echo filePath &" in "& curPath 
	'V8Unpack.exe -unpack      %2                              %2.unp
	'V8Unpack.exe -undeflate   %2.unp\metadata.data            %2.unp\metadata.data.und
	'V8Unpack.exe -unpack      %2.unp\metadata.data.und        %2.unp\smetadata.unp
	commandLineUnpack = "..\Util\v8unpack\v8Unpack.exe -parse "&curPath&"\"&filePath&" "&curPath&"\"&filePath&".src --PARSELEVEL=2"
	
	Dim oShells
	Set oShell = WScript.CreateObject ("WScript.Shell")
	oShell.Run(commandLineUnpack)
	
	Set oShell = Nothing

End Function
