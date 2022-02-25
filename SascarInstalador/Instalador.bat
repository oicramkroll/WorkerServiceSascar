mkdir "c:/sascarService"
xcopy "%~dp0worker\*" "c:\sascarService\." /s /y
sc.exe create "svc worker sascar" binpath="c:/sascarService/WorkerServiceSascar.exe"

"Inicializando servico"
sc start "svc worker sascar"
:loop
sc.exe query "svc worker sascar"| find "RUNNING"
if errorlevel 1 (
  timeout 1
  goto loop
)

@echo off
set SCRIPT="%TEMP%\%RANDOM%-%RANDOM%-%RANDOM%-%RANDOM%.vbs"
echo Set oWS = WScript.CreateObject("WScript.Shell") >> %SCRIPT%
echo sLinkFile = "%USERPROFILE%\Desktop\SasCarReport.lnk" >> %SCRIPT%
echo Set oLink = oWS.CreateShortcut(sLinkFile) >> %SCRIPT%
echo oLink.TargetPath = "c:\sascarService\ManagerReport.exe" >> %SCRIPT%
echo oLink.Save >> %SCRIPT%
cscript /nologo %SCRIPT%
del %SCRIPT%

pause "Servico 'svc worker sascar' instalado com sucesso."
