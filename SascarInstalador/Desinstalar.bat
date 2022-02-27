@echo "Parando o servico..."

@echo off
sc.exe stop "svc worker sascar"
:loop
sc.exe query "svc worker sascar"| find "STOPPED"
if errorlevel 1 (
  timeout 1
  goto loop
)

@echo "Removendo o servico..."
@echo off
sc.exe delete "svc worker sascar"
@echo "Removendo dependencias"

@echo off
rmdir /S /Q "c:\sascarService"
del /S /Q "%USERPROFILE%\Desktop\SasCarReport.lnk"
del /S /Q "%USERPROFILE%\Desktop\ConfigFile.lnk"

pause "servico removido"
