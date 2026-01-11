# ⚡ GUIA RÁPIDO - Verificar Deploy em 2 Minutos

**🎯 Objetivo:** Descobrir por que o app não abre

---

## 📱 PASSO 1: GitHub Actions (30 segundos)

### Acesse:
```
https://github.com/avilaops/hotelaria/actions
```

### Veja o topo da lista:

#### ✅ Se estiver VERDE:
```
✅ Latest commit: Build and deploy... ✓
   └─ O deploy funcionou!
   └─ Problema está no Azure (app não está iniciando)
   └─ Vá para CENÁRIO A abaixo
```

#### ❌ Se estiver VERMELHO:
```
❌ Latest commit: Build and deploy... ✗
   └─ O deploy falhou!
   └─ Nem chegou ao Azure
   └─ Vá para CENÁRIO B abaixo
```

---

## 🔄 CENÁRIO A: GitHub Verde (deploy OK, app não inicia)

### Problema:
O código foi enviado para o Azure, mas o app não está rodando.

### Causas Possíveis:
1. App crashou ao iniciar
2. Variáveis de ambiente faltando
3. App Service stopped

### Solução:

#### Opção 1: Executar script (FÁCIL)
```powershell
.\azure-login-logs.ps1
```
O script vai mostrar os logs e o erro exato.

#### Opção 2: Azure Portal (MANUAL)
```
1. Acesse: https://portal.azure.com
2. Procure: "hotelaria-app"
3. Clique no App Service
4. Menu esquerdo → "Log stream"
5. Veja os erros em tempo real
```

### Erros Comuns:

| Erro no Log | Causa | Solução |
|-------------|-------|---------|
| `connection string` | MongoDB não configurado | Configurar MONGO_CONNECTION_STRING |
| `port already in use` | Configuração de porta errada | Verificar Program.cs |
| `assembly not found` | DLL faltando | Fazer rebuild e deploy |
| `null reference` | Variável de ambiente faltando | Configurar no Azure |

---

## 🔴 CENÁRIO B: GitHub Vermelho (deploy falhou)

### Problema:
O código nem chegou ao Azure. Falhou no build/deploy.

### Como Ver o Erro:

```
1. GitHub Actions → Clique no workflow vermelho
2. Veja a lista de "Jobs"
3. Clique no job vermelho (geralmente "build" ou "deploy")
4. Expanda o step vermelho
5. Leia a mensagem de erro
```

### Erros Comuns:

| Erro | Causa | Solução |
|------|-------|---------|
| `CS0103` ou `CS****` | Erro de código C# | Corrigir código |
| `Unable to connect to Azure` | Credenciais inválidas | Reconfigurar secrets |
| `Authentication failed` | Service Principal expirado | Renovar credentials |
| `Build failed` | Erro de compilação | Ver detalhes do erro |

### Solução Rápida:

Se for erro de código:
```powershell
# Ver erros localmente
dotnet build

# Corrigir os erros
# Então commit e push
git add .
git commit -m "Fix build errors"
git push
```

Se for erro de credenciais:
```
Precisa reconfigurar os secrets do GitHub
Ver: docs/AZURE-PUBLISH-PROFILE-GUIA.md
```

---

## 🎯 AÇÃO IMEDIATA

### Passo 1: Verificar GitHub (30 seg)
```
https://github.com/avilaops/hotelaria/actions
→ Verde ou Vermelho?
```

### Passo 2: Me informar
```
"GitHub está [VERDE/VERMELHO]"
```

### Passo 3: Se vermelho
```
"O erro é: [copiar primeira linha do erro]"
```

### Passo 4: Se verde
```
Execute: .\azure-login-logs.ps1
Ou me diga qual erro aparece ao acessar o site
```

---

## 📊 TABELA DE DECISÃO

| GitHub Actions | App no Browser | Diagnóstico | Próxima Ação |
|----------------|----------------|-------------|--------------|
| ✅ Verde | ❌ Timeout/503 | App crashou | Ver logs Azure |
| ✅ Verde | ❌ 500 Error | Erro interno | Ver logs Azure |
| ✅ Verde | ✅ Funciona | Tudo OK | 🎉 |
| ❌ Vermelho | ❌ Qualquer | Deploy falhou | Ver logs GitHub |

---

## 💡 DICA PRO

Mantenha estas 2 abas abertas sempre:

1. **GitHub Actions**
   ```
   https://github.com/avilaops/hotelaria/actions
   ```
   Para ver se o deploy passou

2. **Azure Log Stream**
   ```
   Portal Azure → hotelaria-app → Log stream
   ```
   Para ver se o app está rodando

---

## ⏱️ TIMING NORMAL

Após fazer `git push`:

```
0:00 - Push completo
0:01 - GitHub Actions inicia
0:05 - Build completo
0:08 - Deploy para Azure
0:10 - App inicia no Azure
0:12 - App acessível ✅
```

**Total: ~12 minutos**

Se passou disso e não funcionou, algo está errado.

---

## 🆘 PRECISA DE AJUDA?

Me envie UMA destas informações:

### Opção 1: Screenshot
```
📸 GitHub Actions mostrando verde/vermelho
```

### Opção 2: Erro do GitHub
```
📝 Mensagem de erro do workflow
```

### Opção 3: Erro do Browser
```
📝 Mensagem que aparece ao acessar o site
```

### Opção 4: Logs do Azure
```
📋 Output do script azure-login-logs.ps1
```

Com qualquer uma dessas informações, posso te ajudar! 🚀

---

## 🔗 Links Úteis

| Recurso | Link | Tempo |
|---------|------|-------|
| GitHub Actions | https://github.com/avilaops/hotelaria/actions | 30s |
| Azure Portal | https://portal.azure.com | 2min |
| Seu App | https://hotelaria-app.azurewebsites.net | 5s |

---

**Resumindo:** Acesse o GitHub Actions e me diga se está verde ou vermelho! 🎯
