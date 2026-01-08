@echo off
echo ==========================================
echo  Corrigindo Erros de Compilacao
echo ==========================================
echo.

echo [1/3] Limpando cache de build...
if exist obj rmdir /s /q obj
if exist bin rmdir /s /q bin
echo ✓ Cache limpo

echo.
echo [2/3] Restaurando dependencias...
dotnet restore
echo ✓ Dependencias restauradas

echo.
echo [3/3] Compilando projeto...
dotnet build --no-incremental
echo.

if %ERRORLEVEL% EQU 0 (
    echo ==========================================
    echo  ✓ Compilacao bem-sucedida!
    echo ==========================================
    echo.
    echo Deseja rodar o sistema? (S/N)
    set /p resposta=
    if /i "%resposta%"=="S" (
        echo.
        echo Iniciando sistema...
        echo Acesse: http://localhost:5000
        echo.
        dotnet run
    )
) else (
    echo ==========================================
    echo  ✗ Erro na compilacao
    echo ==========================================
    echo.
    echo Verifique os erros acima e corrija manualmente.
    echo.
)

pause
