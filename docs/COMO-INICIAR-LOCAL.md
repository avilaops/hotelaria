# 🚀 Como Iniciar o Sistema Hotelaria Localmente

**Última atualização:** 09/01/2026 às 16:20

---

## ⚡ MÉTODO RÁPIDO (Recomendado)

### Opção 1: PowerShell Script

```powershell
# No PowerShell (no diretório D:\Hotelaria)
.\iniciar.ps1
```

### Opção 2: Batch Script

```cmd
# No CMD (no diretório D:\Hotelaria)
iniciar-local.bat
```

### Opção 3: Comando Direto

```powershell
# No PowerShell
dotnet watch run --urls "http://localhost:5000;https://localhost:5001"
```

---

## 📋 PASSO A PASSO DETALHADO

### 1. Abrir Terminal

**Windows PowerShell:**
- Pressione `Win + X`
- Escolha "Windows PowerShell" ou "Terminal"

**Ou:**
- Abra o Explorador de Arquivos
- Navegue até `D:\Hotelaria`
- Digite `powershell` na barra de endereço
- Pressione Enter

### 2. Navegar até o Diretório

```powershell
cd D:\Hotelaria
```

### 3. Executar o Script

```powershell
.\iniciar.ps1
```

**Ou diretamente:**

```powershell
dotnet run
```

### 4. Aguardar Compilação

Você verá algo como:

```
Building...
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5001
```

### 5. Abrir no Navegador

```
http://localhost:5000
```

Ou com HTTPS:

```
https://localhost:5001
```

---

## 🎯 URLS DE ACESSO

| Protocolo | URL | Recomendado |
|-----------|-----|-------------|
| HTTP | http://localhost:5000 | ✅ Sim |
| HTTPS | https://localhost:5001 | ⚠️ Pode dar aviso de certificado |

---

## ⚙️ CONFIGURAÇÕES

### Com Hot Reload (Recomendado)

```powershell
dotnet watch run
```

**Benefício:** Recarrega automaticamente quando você salvar arquivos.

### Sem Hot Reload

```powershell
dotnet run
```

**Uso:** Quando hot reload está causando problemas.

### Porta Específica

```powershell
dotnet run --urls "http://localhost:8080"
```

### Production Mode

```powershell
dotnet run --configuration Release
```

---

## 🔧 SOLUÇÃO DE PROBLEMAS

### Problema: "Porta já em uso"

**Erro:**
```
Failed to bind to address http://127.0.0.1:5000
```

**Solução 1: Matar o processo**
```powershell
# Encontrar processo usando a porta
netstat -ano | findstr :5000

# Matar o processo (substitua PID pelo número encontrado)
taskkill /PID <PID> /F
```

**Solução 2: Usar outra porta**
```powershell
dotnet run --urls "http://localhost:5555"
```

### Problema: "Erro de compilação"

**Solução:**
```powershell
# Limpar e rebuild
dotnet clean
dotnet build
dotnet run
```

### Problema: "MongoDB não conecta"

**Causa:** Variável MONGO_ATLAS_URI não configurada ou inválida.

**Solução:**
1. Verifique `.env` tem `MONGO_ATLAS_URI`
2. Confirme que a connection string está correta
3. Teste a conexão: https://cloud.mongodb.com

### Problema: "Certificado HTTPS inválido"

**Solução:**
```powershell
# Confiar no certificado de desenvolvimento
dotnet dev-certs https --trust
```

---

## 🎨 ABRIR NO VS CODE (Opcional)

```powershell
# Abrir VS Code no diretório atual
code .
```

Depois no terminal integrado do VS Code:
```powershell
dotnet run
```

---

## 🖥️ ABRIR NO VISUAL STUDIO (Opcional)

1. Abra Visual Studio
2. File → Open → Project/Solution
3. Navegue até `D:\Hotelaria\Hotelaria.csproj`
4. Pressione `F5` ou clique em "Play" (▶️)

---

## 📊 MONITORAMENTO

### Ver Logs Detalhados

```powershell
dotnet run --verbosity detailed
```

### Ver Apenas Erros

```powershell
dotnet run --verbosity quiet
```

---

## ⌨️ ATALHOS ÚTEIS

| Atalho | Função |
|--------|--------|
| `Ctrl + C` | Parar o servidor |
| `Ctrl + R` | Reload (se usando watch) |
| `F5` | Abrir no browser (VS Code) |
| `Shift + F5` | Parar debug (VS) |

---

## 🧪 MODO DE TESTE

### Com InMemory Database (Sem MongoDB)

Edite `Program.cs` temporariamente:

```csharp
// Comente a linha MongoDB e use InMemory
// builder.Services.AddSingleton<MongoDBService>();
builder.Services.AddSingleton<MockDatabaseService>();
```

### Com Dados de Teste

O sistema já vem com usuários de teste:
- **admin** / **admin123**
- **gerente** / **gerente123**
- **recepcao** / **recepcao123**

---

## 📱 TESTAR EM DISPOSITIVOS MÓVEIS

### 1. Descobrir seu IP local

```powershell
ipconfig
```

Procure por "IPv4 Address" (exemplo: 192.168.1.100)

### 2. Iniciar com bind em todos IPs

```powershell
dotnet run --urls "http://0.0.0.0:5000"
```

### 3. Acessar do celular/tablet

```
http://192.168.1.100:5000
```

**Importante:** Dispositivo deve estar na mesma rede WiFi!

---

## 🔐 VARIÁVEIS DE AMBIENTE

O sistema lê automaticamente de `.env`:

```env
MONGO_ATLAS_URI=...
AIRBNB_CLIENT_KEY=...
PAYPAL_ID=...
SENTRY_TOKEN_API=...
```

**Verificar se está carregando:**
```csharp
// No Program.cs, adicione temporariamente:
Console.WriteLine($"MongoDB URI: {builder.Configuration["MONGO_ATLAS_URI"]}");
```

---

## 📈 PERFORMANCE

### Compilação Mais Rápida

```powershell
# Pular restore se já foi feito
dotnet run --no-restore
```

### Build Incremental

```powershell
# Não rebuild tudo
dotnet run --no-build
```

---

## 🎯 CHECKLIST ANTES DE INICIAR

- [ ] Está no diretório `D:\Hotelaria`
- [ ] `.env` existe e está configurado
- [ ] Porta 5000/5001 está livre
- [ ] .NET 8 SDK instalado (`dotnet --version`)
- [ ] Conexão com internet (se usar MongoDB Atlas)

---

## ✅ SUCESSO!

Quando ver esta mensagem:

```
Now listening on: http://localhost:5000
Application started. Press Ctrl+C to shut down.
```

**Seu sistema está rodando!** 🎉

Abra o navegador e acesse:
```
http://localhost:5000
```

---

## 📞 AJUDA ADICIONAL

### Logs não mostram nada?

```powershell
# Modo verbose
$env:ASPNETCORE_LOGGING__LOGLEVEL__DEFAULT="Debug"
dotnet run
```

### Quer resetar tudo?

```powershell
# Limpar tudo e recomeçar
dotnet clean
Remove-Item -Recurse -Force bin, obj
dotnet restore
dotnet build
dotnet run
```

### Performance ruim?

```powershell
# Modo Release (mais rápido)
dotnet run -c Release
```

---

## 🚀 PRÓXIMOS PASSOS

Depois que o sistema abrir:

1. **Login:** Use `admin` / `admin123`
2. **Testar:** Navegue pelas páginas
3. **Debug:** Se algo não funcionar, veja os logs no terminal
4. **Parar:** Pressione `Ctrl+C` no terminal

---

**Dica:** Mantenha este guia aberto em uma aba do navegador para referência rápida! 📖
