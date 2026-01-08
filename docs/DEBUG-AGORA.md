# 🔍 DEBUG RÁPIDO: Login no Azure

## 🎯 O Que Fazer AGORA

### 1. **Aguardar Deploy** (~2-3 min)

```
https://github.com/avilaops/hotelaria/actions
```

Aguardar workflow "Build & Deploy" terminar.

---

### 2. **Abrir Console do Navegador**

1. Ir para: `https://hotelaria.avila.inc/login`
2. Pressionar **F12** (DevTools)
3. Aba **Console**
4. Deixar aberto

---

### 3. **Tentar Login**

**Credenciais corretas:**

```
Usuário: nicolasrosaab
Senha: 7Aciqgr7@
```

⚠️ **ATENÇÃO:** São apenas **1 arroba** `@`, não 2!

---

### 4. **Ver Logs no Console**

Você verá logs como:

```
[LOGIN] === INÍCIO DO LOGIN ===
[LOGIN] Usuário: nicolasrosaab
[LOGIN] Chamando AuthService.Login()...
[LOGIN] Resultado: true
[LOGIN] ✅ Login bem-sucedido!
[LOGIN] Redirecionando para /
[LOGIN] === FIM DO LOGIN ===
```

**Se aparecer erro:**
```
[LOGIN] 🔥 ERRO CRÍTICO: ...
[LOGIN] Stack: ...
```

**Copie e cole aqui!**

---

### 5. **Se Não Funcionar**

#### Opção A: Testar com Admin

```
Usuário: admin
Senha: admin123
```

#### Opção B: Ver Logs do Azure

```
https://portal.azure.com
→ hotelaria-app
→ Log stream
```

Procurar por:
- `[LOGIN]`
- `Exception`
- `Error`

#### Opção C: Reiniciar App

```powershell
az webapp restart --name hotelaria-app --resource-group hotelaria-rg
```

Ou no Portal:
```
hotelaria-app → Overview → Restart
```

---

## 🐛 Possíveis Erros

### 1. "Blazor: Connection to server disconnected"

**Causa:** SignalR perdeu conexão.

**Solução:**
- Refresh da página (F5)
- Aguardar 30 segundos
- Tentar novamente

---

### 2. Botão "Entrar" não responde

**Causa:** JavaScript não está executando.

**Verificar:**
```javascript
// No console do navegador
console.log(Blazor);
```

Se retornar `undefined`, significa que Blazor não carregou.

---

### 3. "Usuário ou senha incorretos"

**Causas:**
- Senha errada (verificar @ único)
- AuthService não encontrou usuário
- Problema no hash da senha

**Testar:**
```
admin / admin123
```

Se este funcionar, problema é na senha do `nicolasrosaab`.

---

## 📋 Checklist

- [ ] Deploy concluído no GitHub Actions
- [ ] Página carregada: `https://hotelaria.avila.inc/login`
- [ ] Console aberto (F12)
- [ ] Tentou login com: `nicolasrosaab / 7Aciqgr7@`
- [ ] Viu logs no console começando com `[LOGIN]`
- [ ] Se erro, copiar mensagem completa

---

## 🚀 Após Deploy (2-3 min)

1. **Refresh da página** `https://hotelaria.avila.inc/login`
2. **F12** para abrir console
3. **Tentar login**
4. **Ver logs** no console
5. **Reportar resultado** aqui

---

**⏱️ Aguardando deploy... (~2-3 minutos)**

Quando terminar, siga os passos acima e me diga o que aparece no console! 🔍
