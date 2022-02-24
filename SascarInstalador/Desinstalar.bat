sc.exe stop "svc worker sascar"
:loop
sc.exe query "svc worker sascar"| find "STOPPED"
if errorlevel 1 (
  timeout 1
  goto loop
)


sc.exe delete "svc worker sascar"
rmdir /S /Q "c:\sascarService"
pause "servico removido"
