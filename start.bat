@echo off
REM Script de inicialização rápida do Hotelaria (Windows)

echo 🏨 Bem-vindo ao Hotelaria - Sistema de Gestão Hoteleira
echo.

REM Verificar se o .NET está instalado
where dotnet >nul 2>nul
if %ERRORLEVEL% NEQ 0 (
    echo ❌ .NET SDK não encontrado!
    echo Por favor, instale o .NET 8.0 SDK de https://dotnet.microsoft.com/download/dotnet/8.0
    pause
    exit /b 1
)

echo ✅ .NET SDK encontrado
dotnet --version
echo.

REM Restaurar dependências
echo 📦 Restaurando dependências...
dotnet restore
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Erro ao restaurar dependências
    pause
    exit /b 1
)
echo ✅ Dependências restauradas com sucesso
echo.

REM Compilar projeto
echo 🔨 Compilando projeto...
dotnet build
if %ERRORLEVEL% NEQ 0 (
    echo ❌ Erro ao compilar projeto
    pause
    exit /b 1
)
echo ✅ Projeto compilado com sucesso
echo.

REM Executar aplicação
echo 🚀 Iniciando aplicação...
echo.
echo A aplicação estará disponível em:
echo   • HTTPS: https://localhost:5001
echo   • HTTP:  http://localhost:5000
echo.
echo Pressione Ctrl+C para parar a aplicação
echo.

dotnet run
pause
