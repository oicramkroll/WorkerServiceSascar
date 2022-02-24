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

pause "Servico 'svc worker sascar' instalado com sucesso."
