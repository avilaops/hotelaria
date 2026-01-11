# 🔒 Habilitar HTTPS Only no Azure

## Por que habilitar?

Atualmente o app aceita **HTTP e HTTPS**. Isso pode causar:
- ❌ Credenciais enviadas sem criptografia (HTTP)
- ❌ Vulnerabilidade a ataques man-in-the-middle
- ⚠️ Alerta de segurança em navegadores modernos

---

## ✅ Como Habilitar (2 minutos)

### Via Azure Portal:

1. **Acesse:** https://portal.azure.com
2. **Vá para:** `hotelaria-app`
3. **Menu lateral → "Configuração" → "Configurações gerais"**
4. **Procure:** "HTTPS Only"
5. **Altere para:** **ON** ✅
6. **Clique:** "Salvar"

---

### Via Azure CLI (se preferir):

```powershell
az webapp update `
  --name hotelaria-app `
  --resource-group hotelaria-rg `
  --set httpsOnly=true
```

---

## 📊 Resultado Esperado

### ANTES:
- ✅ http://hotelaria.avila.inc → Funciona
- ✅ https://hotelaria.avila.inc → Funciona

### DEPOIS:
- ❌ http://hotelaria.avila.inc → **Redireciona automaticamente para HTTPS**
- ✅ https://hotelaria.avila.inc → Funciona

---

## 🎯 Benefícios

1. **Segurança:**
   - 🔒 Todas as credenciais criptografadas
   - 🔒 Sessões protegidas
   - 🔒 Dados sensíveis seguros

2. **SEO:**
   - 🚀 Google favorece sites HTTPS
   - 🚀 Melhor ranking

3. **Confiança:**
   - ✅ Navegadores mostram "Conexão Segura"
   - ✅ Sem alertas de segurança

---

## ⚠️ Impacto

- **Tempo de inatividade:** Nenhum
- **Compatibilidade:** 100% (redireciona automaticamente)
- **Performance:** Sem impacto

---

## 📝 Verificação

Após habilitar, teste:

1. **Acesse:** http://hotelaria.avila.inc
2. **Deve redirecionar para:** https://hotelaria.avila.inc
3. **Verificar:** Cadeado aparece no navegador 🔒

---

**Recomendação:** Habilite agora! É rápido e aumenta a segurança significativamente.

---

**Data:** 09/01/2026  
**Status:** ⚠️ Recomendação de Segurança
