#!/bin/bash

# Script de inicialização rápida do Hotelaria

echo "🏨 Bem-vindo ao Hotelaria - Sistema de Gestão Hoteleira"
echo ""

# Verificar se o .NET está instalado
if ! command -v dotnet &> /dev/null
then
    echo "❌ .NET SDK não encontrado!"
    echo "Por favor, instale o .NET 8.0 SDK de https://dotnet.microsoft.com/download/dotnet/8.0"
    exit 1
fi

echo "✅ .NET SDK encontrado: $(dotnet --version)"
echo ""

# Restaurar dependências
echo "📦 Restaurando dependências..."
dotnet restore
if [ $? -ne 0 ]; then
    echo "❌ Erro ao restaurar dependências"
    exit 1
fi
echo "✅ Dependências restauradas com sucesso"
echo ""

# Compilar projeto
echo "🔨 Compilando projeto..."
dotnet build
if [ $? -ne 0 ]; then
    echo "❌ Erro ao compilar projeto"
    exit 1
fi
echo "✅ Projeto compilado com sucesso"
echo ""

# Executar aplicação
echo "🚀 Iniciando aplicação..."
echo ""
echo "A aplicação estará disponível em:"
echo "  • HTTPS: https://localhost:5001"
echo "  • HTTP:  http://localhost:5000"
echo ""
echo "Pressione Ctrl+C para parar a aplicação"
echo ""

dotnet run
