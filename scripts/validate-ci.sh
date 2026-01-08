#!/bin/bash

# 🧪 Script de Validação Local para CI/CD
# Este script simula os checks que o GitHub Actions fará

echo "🚀 Iniciando validação local do projeto Hotelaria..."
echo ""

# Cores para output
GREEN='\033[0;32m'
RED='\033[0;31m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Contador de erros
ERRORS=0

# ============================================
# 1. Verificar .NET SDK
# ============================================
echo -e "${YELLOW}📋 [1/6] Verificando .NET SDK...${NC}"
if command -v dotnet &> /dev/null; then
    DOTNET_VERSION=$(dotnet --version)
    echo -e "${GREEN}✅ .NET SDK encontrado: v${DOTNET_VERSION}${NC}"
else
    echo -e "${RED}❌ .NET SDK não encontrado!${NC}"
    ERRORS=$((ERRORS + 1))
fi
echo ""

# ============================================
# 2. Restore Dependencies
# ============================================
echo -e "${YELLOW}📦 [2/6] Restaurando dependências...${NC}"
if dotnet restore; then
    echo -e "${GREEN}✅ Dependências restauradas com sucesso${NC}"
else
    echo -e "${RED}❌ Falha ao restaurar dependências${NC}"
    ERRORS=$((ERRORS + 1))
fi
echo ""

# ============================================
# 3. Build
# ============================================
echo -e "${YELLOW}🔨 [3/6] Compilando projeto...${NC}"
if dotnet build --no-restore --configuration Release /p:TreatWarningsAsErrors=false; then
    echo -e "${GREEN}✅ Build concluído com sucesso${NC}"
else
    echo -e "${RED}❌ Falha no build${NC}"
    ERRORS=$((ERRORS + 1))
fi
echo ""

# ============================================
# 4. Run Tests
# ============================================
echo -e "${YELLOW}🧪 [4/6] Executando testes...${NC}"
if dotnet test --no-build --configuration Release --verbosity minimal; then
    echo -e "${GREEN}✅ Todos os testes passaram${NC}"
else
    echo -e "${RED}❌ Alguns testes falharam${NC}"
    ERRORS=$((ERRORS + 1))
fi
echo ""

# ============================================
# 5. Verify Publish
# ============================================
echo -e "${YELLOW}📤 [5/6] Testando publicação...${NC}"
if dotnet publish --no-build --configuration Release --output ./publish-test; then
    echo -e "${GREEN}✅ Publicação bem-sucedida${NC}"
    # Cleanup
    rm -rf ./publish-test
else
    echo -e "${RED}❌ Falha na publicação${NC}"
    ERRORS=$((ERRORS + 1))
fi
echo ""

# ============================================
# 6. Check Git Status
# ============================================
echo -e "${YELLOW}📂 [6/6] Verificando status do Git...${NC}"
if [ -n "$(git status --porcelain)" ]; then
    echo -e "${YELLOW}⚠️  Existem arquivos não commitados:${NC}"
    git status --short
else
    echo -e "${GREEN}✅ Working tree limpo${NC}"
fi
echo ""

# ============================================
# Summary
# ============================================
echo "=================================="
echo "📊 RESUMO DA VALIDAÇÃO"
echo "=================================="

if [ $ERRORS -eq 0 ]; then
    echo -e "${GREEN}✅ Todas as verificações passaram!${NC}"
    echo -e "${GREEN}🚀 Você pode fazer push com segurança${NC}"
    exit 0
else
    echo -e "${RED}❌ $ERRORS verificação(ões) falharam${NC}"
    echo -e "${RED}🛑 Corrija os erros antes de fazer push${NC}"
    exit 1
fi
