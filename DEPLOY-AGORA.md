# ⚡ DEPLOY AGORA - 3 Comandos

## 🎯 O QUE FAZER AGORA:

### 1. Autenticar Azure (1 vez só)
```powershell
az login
```
*Uma janela vai abrir, faça login*

### 2. Configurar Variáveis (5 min)
```powershell
cd D:\Hotelaria
.\configure-azure-env.ps1
```

### 3. Deploy (15 min)
```bash
git add .
git commit -m "fix: Update to .NET 9.0 for Azure compatibility"
git push origin main
```

---

## 🔍 Por Que Não Funcionou Antes?

**Problema identificado nos logs:**
```
Framework: 'Microsoft.NETCore.App', version '8.0.0' (x64)
The following frameworks were found:
  9.0.10 at [/usr/share/dotnet/shared/Microsoft.NETCore.App]
```

**Causa:** Azure tem .NET 9.0, projeto estava em .NET 8.0

**Solução:** ✅ JÁ APLICADA
- `Hotelaria.csproj` atualizado para .NET 9.0
- GitHub Actions atualizado para .NET 9.0

---

## ✅ O Que Já Foi Feito:

- ✅ Projeto atualizado para .NET 9.0
- ✅ GitHub Actions atualizado
- ✅ Health checks implementados
- ✅ Program.cs otimizado para Azure
- ✅ Scripts de deploy criados

## ⏳ O Que Falta:

- [ ] Você executar `az login`
- [ ] Você executar `.\configure-azure-env.ps1`
- [ ] Você fazer `git push`

---

## 🚀 Comando Único (Depois do az login):

```powershell
cd D:\Hotelaria ; .\configure-azure-env.ps1 ; git add . ; git commit -m "fix: .NET 9.0" ; git push origin main
```

---

**Tempo Total:** 20 minutos (5 config + 15 GitHub Actions)
