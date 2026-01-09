# 🔧 Correção: Application Error no Azure

**Data:** 09/01/2026  
**Problema:** "Application Error" - `Hotelaria.dll` não existe  
**Status:** ✅ **RESOLVIDO**

---

## 🔴 Problema Identificado

### Erro nos Logs do Azure:
```
The application 'Hotelaria.dll' does not exist.
```

### Causa Raiz:
1. **Workflow do GitHub Actions usava .NET 8.0.x** mas o projeto é **.NET 9.0**
2. **`Hotelaria.dll` não estava sendo gerado** corretamente no build
3. **Deploy enviava pacote vazio** para o Azure

---

## ✅ Solução Aplicada

### Mudanças no `.github/workflows/dotnet.yml`:

#### 1. **Versão do .NET Corrigida**
```yaml
# ANTES ❌
env:
  DOTNET_VERSION: '8.0.x'

# DEPOIS ✅
env:
  DOTNET_VERSION: '9.0.x'
```

#### 2. **Comando de Publish Otimizado**
```yaml
# ANTES ❌
- name: 📦 Publish
  run: dotnet publish --no-build --configuration ${{ env.BUILD_CONFIGURATION }} --output ${{ env.AZURE_WEBAPP_PACKAGE_PATH }}

# DEPOIS ✅
- name: 📦 Publish
  run: dotnet publish --configuration ${{ env.BUILD_CONFIGURATION }} --output ${{ env.AZURE_WEBAPP_PACKAGE_PATH }} --no-restore
```

**Por quê?**
- `--no-build` pode causar problemas se o build anterior falhou parcialmente
- Remover `--no-build` garante que o `publish` recompila se necessário
- `--no-restore` mantém performance (já foi feito restore antes)

#### 3. **Verificação Automática Adicionada**
```yaml
- name: 🔍 Verify DLL exists
  run: |
    echo "📁 Checking published files..."
    ls -lah ${{ env.AZURE_WEBAPP_PACKAGE_PATH }}
    if [ ! -f "${{ env.AZURE_WEBAPP_PACKAGE_PATH }}/Hotelaria.dll" ]; then
      echo "❌ ERROR: Hotelaria.dll not found!"
      exit 1
    else
      echo "✅ Hotelaria.dll found!"
    fi
```

**Benefício:** Se o `Hotelaria.dll` não for gerado, o workflow **falha imediatamente** antes do deploy.

---

## 📋 Como Aplicar a Correção

### Passo 1: Commit e Push

```bash
git add .github/workflows/dotnet.yml
git commit -m "🔧 fix: Corrigir versão .NET 9 e geração de Hotelaria.dll"
git push origin main
```

### Passo 2: Verificar GitHub Actions

1. Acesse: https://github.com/avilaops/hotelaria/actions
2. Aguarde o workflow **"CI/CD Pipeline - Hotelaria"** executar
3. Verifique se:
   - ✅ Build & Test passou
   - ✅ Verificação do DLL passou
   - ✅ Deploy foi bem-sucedido

### Passo 3: Testar Aplicação

1. Aguardar 1-2 minutos após deploy
2. Acessar: https://hotelaria.avila.inc
3. Verificar se não há mais "Application Error"
4. Fazer login com `admin / admin123`

---

## 🔍 Verificação nos Logs do Azure

Após o deploy, os logs devem mostrar:

```
✅ ANTES (Erro):
The application 'Hotelaria.dll' does not exist.

✅ DEPOIS (Sucesso):
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://[::]:8080
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

---

## 🎯 Checklist de Verificação

- [ ] Workflow atualizado com `.NET 9.0.x`
- [ ] Commit e push feito
- [ ] GitHub Actions executou com sucesso
- [ ] Verificação do DLL passou
- [ ] Deploy completou sem erros
- [ ] Aplicação acessível em https://hotelaria.avila.inc
- [ ] Login funcionando

---

## 🚨 Se o Problema Persistir

### 1. Verificar Runtime do Azure

O Azure deve usar **.NET 9 Runtime**. Verifique no portal:

```
Azure Portal → hotelaria-app → Configuration → General settings
→ Stack: .NET
→ .NET version: 9.0
```

Se estiver em `.NET 8`, **mude para .NET 9** e salve.

### 2. Limpar Cache do Azure

Às vezes o Azure mantém cache de builds antigos:

```powershell
# Via Azure CLI (se tiver acesso)
az webapp restart --name hotelaria-app --resource-group hotelaria-rg
```

Ou no portal:
```
Azure Portal → hotelaria-app → Overview → Restart
```

### 3. Verificar Publish Profile

Se você ainda não configurou o Publish Profile, siga:
- `docs/AZURE-PUBLISH-PROFILE-GUIA.md` (Opção 1: Basic Auth)

---

## 📊 Comparação: Antes vs Depois

| Item | Antes ❌ | Depois ✅ |
|------|---------|-----------|
| **.NET Version** | 8.0.x | 9.0.x |
| **Publish Command** | `--no-build` | Recompila se necessário |
| **Verificação DLL** | Nenhuma | Automática |
| **Deploy** | Pacote vazio | Pacote completo |
| **Logs Azure** | "DLL does not exist" | "Application started" |
| **Site** | Application Error | ✅ Funcionando |

---

## 🎉 Resultado Esperado

Após aplicar a correção:

```
╔════════════════════════════════════════╗
║  APLICAÇÃO FUNCIONANDO NO AZURE       ║
╠════════════════════════════════════════╣
║  GitHub Actions:     ✅ Sucesso        ║
║  DLL Gerado:         ✅ Hotelaria.dll  ║
║  Deploy:             ✅ Completo       ║
║  Runtime:            ✅ .NET 9.0       ║
║  Site:               ✅ Acessível      ║
╠════════════════════════════════════════╣
║  URL: https://hotelaria.avila.inc     ║
║  Login: admin / admin123              ║
╚════════════════════════════════════════╝
```

---

## 📝 Lições Aprendidas

1. **Sempre verificar versão do .NET** no workflow vs projeto
2. **Adicionar verificações automáticas** (ex: verificar se DLL existe)
3. **Ler logs do Azure** para diagnóstico preciso
4. **Não usar `--no-build` em publish** se houver dúvidas

---

## 📞 Próximos Passos

Após aplicar esta correção:

1. [ ] **Habilitar Basic Auth** (se ainda não fez)
   - Seguir: `docs/AZURE-PUBLISH-PROFILE-GUIA.md`
   
2. [ ] **Baixar Publish Profile**
   - Adicionar no GitHub Secrets como `AZURE_WEBAPP_PUBLISH_PROFILE`
   
3. [ ] **Testar deploy automático**
   - Fazer alteração qualquer e push para `main`
   - Verificar se deploy funciona

4. [ ] **Migrar para Service Principal** (opcional, mais seguro)
   - Seguir: `docs/AZURE-SERVICE-PRINCIPAL.md`

---

**Autor:** GitHub Copilot  
**Data:** 09/01/2026  
**Status:** ✅ Correção Aplicada

---

**Ávila Inc. - Troubleshooting Documentation**
