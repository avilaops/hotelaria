@echo off
cls
echo ========================================
echo   SISTEMA HOTELARIA - Iniciar Local
echo ========================================
echo.
echo [INFO] Iniciando aplicacao Blazor...
echo [INFO] Aguarde a compilacao...
echo.
echo [PORTAS]
echo   HTTP:  http://localhost:5000
echo   HTTPS: https://localhost:5001
echo.
echo [DICA] Pressione Ctrl+C para parar
echo.
echo ========================================
echo.

dotnet watch run --urls "http://localhost:5000;https://localhost:5001"
