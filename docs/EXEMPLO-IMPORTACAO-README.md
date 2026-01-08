# 📄 Arquivo de Exemplo para Importação

## 🎯 Objetivo

Este arquivo CSV serve como **modelo completo** para importação de reservas no sistema de Hotelaria.

## 📋 Conteúdo

O arquivo `exemplo-importacao-completo.csv` contém **5 reservas de teste** com todos os campos preenchidos, incluindo:

### ✅ Campos Obrigatórios
- Nome do hóspede
- Check-in e Check-out
- Número da reserva
- Total da reserva
- Identificação do quarto (via campo "Cama")

### 💡 Campos Recomendados (novos!)
- **Email** - Coluna 18
- **Telefone** - Coluna 19
- Forma de pagamento
- Data de pagamento

### 📊 Reservas de Exemplo

| Hóspede | País | Email | Telefone | Check-in | Check-out | Quarto | Total |
|---------|------|-------|----------|----------|-----------|--------|-------|
| Mohamed Tarek | 🇩🇪 Alemanha | mohamed.tarek@email.com | +49 170 123 4567 | 08/01/2026 | 10/01/2026 | Q 3 | € 43,80 |
| Nassine Salam | 🇹🇳 Tunisia | nassine.salam@email.com | +216 98 765 432 | 08/01/2026 | 12/01/2026 | Q 5 | € 72,00 |
| Ana Silva | 🇧🇷 Brasil | ana.silva@gmail.com | +55 11 98765-4321 | 09/01/2026 | 11/01/2026 | Q 1 | € 50,00 |
| João Santos | 🇵🇹 Portugal | joao.santos@hotmail.com | +351 912 345 678 | 10/01/2026 | 15/01/2026 | Q 2 | € 150,00 |
| Maria Garcia | 🇪🇸 Espanha | maria.garcia@yahoo.es | +34 600 111 222 | 11/01/2026 | 13/01/2026 | Q 4 | € 64,50 |

## 🚀 Como Usar

### **Passo 1: Baixar Arquivo**
```bash
# Arquivo localizado em:
docs/exemplo-importacao-completo.csv
```

### **Passo 2: Abrir no Excel**
1. Abra o arquivo no Excel
2. Verifique que todas as colunas estão visíveis
3. Observe os formatos de data (dd/MM/yyyy)

### **Passo 3: Personalizar (Opcional)**
- Altere nomes dos hóspedes
- Modifique datas conforme necessário
- Ajuste valores e quartos
- Adicione mais linhas seguindo o padrão

### **Passo 4: Salvar**
1. **Arquivo** → **Salvar Como**
2. Escolha **CSV UTF-8 (Separado por vírgulas)**
3. Salve com nome desejado

### **Passo 5: Importar**
1. Acesse: `http://localhost:5000/importar`
2. Clique em **Selecionar Arquivo**
3. Escolha o CSV salvo
4. Revise preview
5. Clique em **Importar**

## ✨ Destaques do Novo Formato

### 📧 Campo Email (Coluna 18)
- **Propósito:** Facilitar comunicação com hóspede
- **Formato:** email@dominio.com
- **Comportamento:**
  - Se CSV tem email → Salva no cadastro do hóspede
  - Se hóspede já existe SEM email → Atualiza com email do CSV
  - Se hóspede já existe COM email → Mantém o existente
  - Se CSV não tem email → Atribui temporário `sem-email@importado.com`

### 📱 Campo Telefone (Coluna 19)
- **Propósito:** Contato rápido com hóspede
- **Formato:** +[código país] [número]
- **Exemplos:**
  - Brasil: `+55 11 98765-4321`
  - Portugal: `+351 912 345 678`
  - Espanha: `+34 600 111 222`
  - Alemanha: `+49 170 123 4567`
- **Comportamento:**
  - Se CSV tem telefone → Salva no cadastro
  - Se hóspede já existe SEM telefone → Atualiza
  - Se hóspede já existe COM telefone → Mantém
  - Se CSV não tem telefone → Atribui "N/A"

## 💡 Benefícios

### **Antes (sem Email/Telefone):**
```
Hóspede: Mohamed Tarek
Email: (vazio)
Telefone: (vazio)
❌ Difícil contatar para confirmações
```

### **Depois (com Email/Telefone):**
```
Hóspede: Mohamed Tarek
Email: mohamed.tarek@email.com
Telefone: +49 170 123 4567
✅ Fácil enviar confirmações por email
✅ Rápido ligar em caso de urgência
✅ Contato disponível para futuras reservas
```

## 🔍 Validações Automáticas

O sistema validará automaticamente:

- ✅ Formato de email (básico)
- ✅ Presença de @ no email
- 💡 Aviso se email estiver vazio
- 💡 Aviso se telefone estiver vazio
- ✅ Criação/atualização inteligente de hóspedes

### Exemplo de Avisos:
```
✅ 5 Linhas Válidas
💡 Aviso linha 2: Email não informado - recomendado para contato
💡 Aviso linha 3: Telefone não informado - recomendado para contato
```

## 📊 Estrutura Completa das Colunas

| Nº | Coluna | Tipo | Obrigatório |
|----|--------|------|-------------|
| 1 | Nome | Texto | ✅ Sim |
| 2 | Nascimento | Data | ⚠️ Opcional |
| 3 | Nº Documento | Texto | ⚠️ Recomendado |
| 4 | País | Texto | ⚠️ Opcional |
| 5 | Tipo Doc | Texto | ⚠️ Opcional |
| 6 | Cama | Texto | ✅ Sim |
| 7 | Check-in | Data | ✅ Sim |
| 8 | Check-out | Data | ✅ Sim |
| 9 | Dias | Número | ⚠️ Opcional |
| 10 | Valor | Decimal | ⚠️ Opcional |
| 11 | Pago | Texto | ⚠️ Opcional |
| 12 | TX Booking | Decimal | ⚠️ Opcional |
| 13 | TX pago | Decimal | ⚠️ Opcional |
| 14 | Nº reserva | Texto | ✅ Sim |
| 15 | Diaria | Decimal | ⚠️ Recomendado |
| 16 | Total | Decimal | ✅ Sim |
| 17 | Livre TX | Decimal | ⚪ Opcional |
| 18 | Diaria Paga | Decimal | ⚪ Opcional |
| **19** | **Email** | **Email** | **💡 Novo!** |
| **20** | **Telefone** | **Texto** | **💡 Novo!** |
| 21 | Forma Pgto | Texto | ⚪ Opcional |
| 22 | Data Pgto | Data | ⚪ Opcional |

## 🎉 Resultado Esperado

Após importação bem-sucedida:

```
✅ Importação Concluída!

📊 Estatísticas:
- 5 reservas importadas
- 5 hóspedes criados/atualizados
- Emails: 5 cadastrados
- Telefones: 5 cadastrados
- Quartos: Q1, Q2, Q3, Q4, Q5

💡 Próximos passos:
1. Verificar hóspedes em "Hóspedes"
2. Conferir reservas em "Reservas"
3. Validar emails e telefones
```

## 🔧 Solução de Problemas

### **Email inválido**
- Sistema aceita qualquer formato
- Recomenda-se formato: `nome@dominio.com`
- Sistema não envia emails automaticamente (apenas armazena)

### **Telefone em formato incorreto**
- Sistema aceita qualquer texto
- Recomenda-se incluir código do país
- Exemplo: `+351 912 345 678` (Portugal)

### **Hóspede duplicado**
- Sistema identifica por documento
- Se documento igual → Atualiza dados (não duplica)
- Email/Telefone atualizados se estiverem vazios

## 📝 Notas Importantes

1. **Formato de Data:** Sempre use `dd/MM/yyyy` (08/01/2026)
2. **Separador Decimal:** Use ponto (`.`) no CSV: `43.80`
3. **Codificação:** Salve como UTF-8 para acentos
4. **Separador de Colunas:** Vírgula (`,`)
5. **Números de Reserva:** Devem ser únicos (TEST001, TEST002, etc.)

## ✅ Checklist de Importação

- [ ] Arquivo tem 21 colunas (incluindo Email e Telefone)
- [ ] Emails em formato válido (com @)
- [ ] Telefones com código do país quando possível
- [ ] Datas em formato dd/MM/yyyy
- [ ] Números de reserva únicos
- [ ] Arquivo salvo como CSV UTF-8
- [ ] Backup dos dados atuais realizado

---

**🎊 Aproveite o novo recurso de Email e Telefone!**

*Com essas informações, você poderá:*
- ✉️ Enviar confirmações por email (futuro)
- 📞 Ligar para hóspedes rapidamente
- 📱 Enviar SMS/WhatsApp (futuro)
- 🎯 Marketing direcionado (futuro)

---

*Última atualização: Janeiro 2026 - v2.6.0*
*Novidade: Suporte a Email e Telefone na importação*
