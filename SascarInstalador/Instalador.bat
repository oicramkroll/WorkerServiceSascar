@echo "Gerando dependencias"
@echo off
mkdir "c:/sascarService"
xcopy "%~dp0worker\*" "c:\sascarService\." /s /y
sc.exe create "svc worker sascar" binpath="c:/sascarService/WorkerServiceSascar.exe"

@echo "Inicializando servico"
sc start "svc worker sascar"

@echo off
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

@echo off
set SCRIPT="%TEMP%\%RANDOM%-%RANDOM%-%RANDOM%-%RANDOM%.vbs"
echo Set oWS = WScript.CreateObject("WScript.Shell") >> %SCRIPT%
echo sLinkFile = "%USERPROFILE%\Desktop\ConfigFile.lnk" >> %SCRIPT%
echo Set oLink = oWS.CreateShortcut(sLinkFile) >> %SCRIPT%
echo oLink.TargetPath = "c:\sascarService\Config.json" >> %SCRIPT%
echo oLink.Save >> %SCRIPT%
cscript /nologo %SCRIPT%
del %SCRIPT%

pause "Servico 'svc worker sascar' instalado com sucesso."
