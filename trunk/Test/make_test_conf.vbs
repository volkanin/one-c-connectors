'maker for test IB '
'������ ������� - ������� ��� ����������� ���� �� ������ ����������� �������� dt ������'
'��� ������������� - ������� �������� HOME ���������� �������� 8.1 � 8.2 �� ���������� ������������'
'��������� ���������� ����� V81_HOME � V82_HOME'
'//TODO - �������� ���������� ����� ������������� ������ Windows �������� ������������ ���� �� "���������" ��������'

strDir = ".\"
Set FSO = CreateObject("Scripting.FileSystemObject")
Set objDir = FSO.GetFolder(strDir)

Set oShell = WScript.CreateObject ("WScript.Shell")

'wscript.Echo objDir'

Set objSystemVariables = oShell.Environment("System") 

v81bin = ""
v82bin = ""

v81bin = objSystemVariables("V81_HOME")
v82bin = objSystemVariables("V82_HOME")

'WScript.StdOut.WriteLine "" & objDir'
'WScript.StdOut.WriteLine " 8.1: " & v81bin & " 8.2: " & v82bin'

result = createIB("""" & v81bin & "\bin\1cv8.exe """, objDir & "\TestIBs\v81test\", objDir & "\MejovAA\Test.dt", "OneCService\TestService81")
result = createIB("""" & v82bin & "\bin\1cv8.exe """, objDir & "\TestIBs\v82test\", objDir & "\MejovAA\Test82.dt", "OneCService\TestService82")'

function createIB(command, ibPath, dtPath, ibName)
	commandCreater = command & " CREATEINFOBASE File=""" & ibPath & """ /AddInList " & ibName
	commandRunner = command & " DESIGNER /F" & ibPath & " /DisableStartupMessages /RestoreIB" & dtPath
	'WScript.StdOut.WriteLine commandCreater'
	retCode = oShell.Run(commandCreater, 1, True)
	'WScript.StdOut.WriteLine commandRunner'
	retCode = oShell.Run(commandRunner, 1, True)
end function

