# 🐛 Debug: Login não Funciona no Azure

**Data:** 08/01/2026  
**Versão:** v2.6.5  
**Problema:** Login não responde ao clicar "Entrar"

---

## 🔍 Diagnóstico

### Problemas Identificados:

#### 1. **Credenciais de Teste Ocultas em Produção**
```razor
@if (isDevelopment)
{
    // Credenciais só aparecem em Development
}
```

**Problema:** No Azure, `ASPNETCORE_ENVIRONMENT = Production`, então as credenciais não aparecem.

**Solução:** Digitar manualmente ou verificar logs.

---

#### 2. **Possível Erro de JavaScript**

O botão "Entrar" pode não estar disparando o evento `@onclick`.

**Causas possíveis:**
- Erro de JavaScript não capturado
- Blazor SignalR desconectado
- Console do navegador com erros

---

## ✅ Soluções

### Solução 1: Verificar Console do Navegador

**Chrome/Edge:**
```
1. Abrir DevTools: F12
2. Console tab
3. Procurar erros em vermelho
4. Copiar erros e enviar
```

**Possíveis erros:**
```
❌ Blazor: Connection to server disconnected
❌ Failed to load resource: _framework/blazor.server.js
❌ SignalR connection error
```

---

### Solução 2: Verificar Logs do Azure

**Azure Portal:**
```
1. hotelaria-app → Log stream
2. Procurar:
   - "Login attempt"
   - "Authentication"
   - Exceptions
```

**Via CLI:**
```powershell
az webapp log tail --name hotelaria-app --resource-group hotelaria-rg
```

---

### Solução 3: Testar Credenciais Corretas

**Usuários válidos no sistema:**

| Usuário | Senha | Perfil |
|---------|-------|--------|
| `nicolasrosaab` | `7Aciqgr7@` | Desenvolvedor |
| `admin` | `admin123` | Administrador |
| `maria` | `maria123` | Gerente |
| `joao` | `joao123` | Recepcionista |

**⚠️ ATENÇÃO:** A senha do desenvolvedor tem apenas **1 arroba** `@`, não 2.

Código correto:
```
Usuário: nicolasrosaab
Senha: 7Aciqgr7@
```

---

### Solução 4: Adicionar Logs de Debug

Editar `Pages/Login.razor`:

**Adicionar no `@code`:**
```csharp
private async Task RealizarLogin()
{
    mensagemErro = string.Empty;
    processando = true;

    // 🔍 DEBUG: Log no console
    await JSRuntime.InvokeVoidAsync("console.log", $"Login attempt: {loginModel.Username}");

    // Validações...
    if (string.IsNullOrWhiteSpace(loginModel.Username))
    {
        mensagemErro = "Por favor, informe o usuário";
        processando = false;
        await JSRuntime.InvokeVoidAsync("console.error", "Username vazio");
        return;
    }

    if (string.IsNullOrWhiteSpace(loginModel.Senha))
    {
        mensagemErro = "Por favor, informe a senha";
        processando = false;
        await JSRuntime.InvokeVoidAsync("console.error", "Senha vazia");
        return;
    }

    // ... resto do código
}
```

---

### Solução 5: Verificar AuthService

Verificar se `AuthService` está registrado:

**`Program.cs`:**
```csharp
builder.Services.AddSingleton<AuthService>();
builder.Services.AddSingleton<UserRepository>();
```

---

### Solução 6: Forçar Reload da Página

Após login bem-sucedido:

```csharp
NavigationManager.NavigateTo("/", forceLoad: true);
```

**⚠️ Se não funcionar, mudar para:**
```csharp
await JSRuntime.InvokeVoidAsync("window.location.href", "/");
```

---

## 🧪 Teste Rápido

### Via Console do Navegador:

**1. Abrir DevTools (F12)**

**2. Console tab**

**3. Testar se Blazor está conectado:**
```javascript
// Verificar se Blazor está carregado
console.log(Blazor);

// Verificar SignalR
console.log(Blazor.defaultReconnectionHandler);
```

**4. Ver estado da conexão:**
```javascript
// Status da conexão SignalR
Blazor._internal.navigationManager.getLocationUrl()
```

---

## 🔧 Correção Completa

### Atualizar `Login.razor`

**Adicionar tratamento de erros:**

```razor
@code {
    // ...existing code...

    private async Task RealizarLogin()
    {
        try
        {
            mensagemErro = string.Empty;
            processando = true;

            // Log de debug
            Console.WriteLine($"[LOGIN] Tentativa de login: {loginModel.Username}");

            // Validações
            if (string.IsNullOrWhiteSpace(loginModel.Username))
            {
                mensagemErro = "Por favor, informe o usuário";
                processando = false;
                return;
            }

            if (string.IsNullOrWhiteSpace(loginModel.Senha))
            {
                mensagemErro = "Por favor, informe a senha";
                processando = false;
                return;
            }

            // Verificar bloqueio
            bloqueadoAte = AuthService.GetLockoutTime(loginModel.Username);
            if (bloqueadoAte.HasValue)
            {
                mensagemErro = $"Conta bloqueada até {bloqueadoAte.Value:HH:mm:ss}";
                processando = false;
                return;
            }

            // Delay anti timing attack
            await Task.Delay(500);

            // Tentar login
            Console.WriteLine($"[LOGIN] Chamando AuthService.Login()...");
            var sucesso = AuthService.Login(loginModel.Username, loginModel.Senha);
            Console.WriteLine($"[LOGIN] Resultado: {sucesso}");

            if (sucesso)
            {
                Console.WriteLine($"[LOGIN] Login bem-sucedido! Redirecionando...");
                
                // Salvar sessão se necessário
                if (loginModel.LembrarMe)
                {
                    try
                    {
                        await JSRuntime.InvokeVoidAsync("sessionStorage.setItem", 
                            "hotelaria_session", 
                            Guid.NewGuid().ToString());
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[LOGIN] Erro ao salvar sessão: {ex.Message}");
                    }
                }

                // Redirecionar
                await Task.Delay(100); // Pequeno delay
                NavigationManager.NavigateTo("/", forceLoad: true);
            }
            else
            {
                Console.WriteLine($"[LOGIN] Login falhou!");
                
                // Atualizar tentativas
                tentativasRestantes = AuthService.GetRemainingAttempts(loginModel.Username);
                bloqueadoAte = AuthService.GetLockoutTime(loginModel.Username);

                if (bloqueadoAte.HasValue)
                {
                    mensagemErro = $"Muitas tentativas. Conta bloqueada por 15 minutos.";
                }
                else if (tentativasRestantes > 0)
                {
                    mensagemErro = "Usuário ou senha incorretos";
                }

                processando = false;
                StateHasChanged(); // Forçar atualização da UI
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[LOGIN] ERRO CRÍTICO: {ex.Message}");
            Console.WriteLine($"[LOGIN] Stack: {ex.StackTrace}");
            mensagemErro = $"Erro ao processar login: {ex.Message}";
            processando = false;
            StateHasChanged();
        }
    }
}
```

---

## 📊 Checklist de Debug

### Verificar:

- [ ] Console do navegador (F12) - Ver erros JavaScript
- [ ] Logs do Azure - Ver erros servidor
- [ ] Credenciais corretas - `nicolasrosaab` / `7Aciqgr7@` (1 arroba)
- [ ] Blazor conectado - SignalR funcionando
- [ ] AuthService registrado - `Program.cs`
- [ ] Botão "Entrar" habilitado - Não está disabled
- [ ] Mensagem de erro - Aparece alguma mensagem?
- [ ] Processando - Spinner aparece?

---

## 🚨 Erros Comuns

### 1. "Connection to server disconnected"

**Causa:** SignalR perdeu conexão.

**Solução:**
```
1. Refresh da página (F5)
2. Verificar logs do Azure
3. Verificar se app não está em cold start
```

---

### 2. Botão não responde

**Causa:** Evento `@onclick` não está disparando.

**Solução:**
```razor
<!-- Adicionar debug -->
<button class="btn-primary-minimal" 
        @onclick="() => { Console.WriteLine(\"Botão clicado!\"); RealizarLogin(); }"
        disabled="@(processando || bloqueadoAte.HasValue)">
    ...
</button>
```

---

### 3. Senha incorreta

**Causa:** Senha tem 1 `@`, não 2.

**Correto:**
```
nicolasrosaab / 7Aciqgr7@
```

**Errado:**
```
nicolasrosaab / 7Aciqgr7@@
```

---

## 🎯 Teste Agora

### Passo 1: Verificar Console

```
1. Abrir https://hotelaria.avila.inc/login
2. F12 (DevTools)
3. Console tab
4. Ver se há erros
```

### Passo 2: Tentar Login

```
Usuário: admin
Senha: admin123
Clicar: Entrar
```

### Passo 3: Ver Logs

**Se não funcionar:**
```
1. Console → Copiar erros
2. Azure Portal → Log stream → Copiar logs
3. Enviar para debug
```

---

## 📞 Próximos Passos

Se nada funcionar:

1. **Enviar logs do console**
2. **Enviar logs do Azure**
3. **Aplicar correção com logs de debug**
4. **Testar novamente**

---

**Versão:** v2.6.5  
**Data:** 08/01/2026  
**Status:** 🐛 Debugging

---

**🔍 Vamos descobrir o problema!**
