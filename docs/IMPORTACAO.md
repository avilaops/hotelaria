# 📥 Guia de Importação de Dados

## 🎯 Visão Geral

O sistema de importação permite que você importe múltiplas reservas de uma vez a partir de arquivos CSV ou TSV (Excel), incluindo:
- Informações dos hóspedes
- Dados das reservas
- Detalhes financeiros
- Criação automática de quartos e hóspedes

---

## 📋 Preparação do Arquivo

### **Passo 1: Exportar do Excel**

1. Abra sua planilha no Excel
2. Selecione **Arquivo** → **Salvar Como**
3. Em **Tipo de arquivo**, escolha:
   - **Texto (Separado por Tabulações) (*.txt)** OU
   - **CSV (Separado por vírgulas) (*.csv)**
4. Clique em **Salvar**

### **Passo 2: Verificar Formato**

Seu arquivo deve ter a seguinte estrutura de colunas:

| Coluna | Descrição | Exemplo |
|--------|-----------|---------|
| Nome | Nome completo do hóspede | Mohamed Tarek Ibrahim |
| Nascimento | Data de nascimento | 19/12/1995 |
| Nº Documento | Número do documento | YZ2PMXCC2 |
| País | País de origem | Alemanha |
| Documento | Tipo (Id/Passaporte) | Passaporte |
| Cama | Quarto e cama | Q 3 - Cama 01 |
| Check-in | Data de entrada | 01/01/2026 |
| Check-out | Data de saída | 03/01/2026 |
| Dias Pessoas | Número | 2 |
| Valor Pago | Valor em € | 40,80 € |
| Pago | Tipo (Online/Cartão) | Online |
| TX Booking | Taxa | 3,00 € |
| TX pago | Taxa paga | - |
| Nº reserva | Código único | 6221128181 |
| Diaria | Valor diária | 20,40 € |
| Total | Valor total | 43,80 € |

---

## 🚀 Processo de Importação

### **Passo 1: Acessar Página de Importação**

1. Acesse: `http://localhost:5000/importar`
2. Ou clique em **📥 Importar Dados** no menu lateral

### **Passo 2: Selecionar Arquivo**

1. Clique em **"Escolher arquivo"**
2. Selecione seu arquivo `.txt`, `.csv` ou `.tsv`
3. O sistema processará automaticamente

### **Passo 3: Revisar Resultado**

O sistema mostrará:

```
✅ 45 Linhas Válidas
❌ 2 Com Erros
📊 47 Total
```

**Erros Comuns:**
- Data inválida
- Nome obrigatório não preenchido
- Check-out anterior ao check-in
- Número de reserva duplicado

### **Passo 4: Preview dos Dados**

Revise as primeiras 5 linhas:

| Nome | Documento | País | Check-in | Check-out | Quarto | Total | Status |
|------|-----------|------|----------|-----------|--------|-------|--------|
| Mohamed... | YZ2PMXCC2 | Alemanha | 01/01/2026 | 03/01/2026 | Q 3 | € 43,80 | ✓ Válido |

### **Passo 5: Confirmar Importação**

1. Clique em **"✅ Importar X Reservas"**
2. Aguarde o processamento
3. Veja mensagem de sucesso
4. Clique em **"Ver Reservas"** para visualizar

---

## 🔍 Validações Automáticas

O sistema valida:

### **Dados Obrigatórios:**
- ✅ Nome do hóspede
- ✅ Data de check-in
- ✅ Data de check-out
- ✅ Número de reserva

### **Regras de Negócio:**
- ✅ Check-out > Check-in
- ✅ Datas em formato válido
- ✅ Valores numéricos positivos

### **Criação Automática:**
- ✅ **Hóspedes novos** (se documento não existir)
- ✅ **Quartos novos** (se número não existir)
- ✅ **Email temporário** (nome@importado.com)

---

## 🎨 Formatos Suportados

### **Datas:**
- `dd/MM/yyyy` (01/01/2026)
- `dd-MM-yyyy` (01-01-2026)
- `yyyy-MM-dd` (2026-01-01)
- `MM/dd/yyyy` (01/01/2026)

### **Valores:**
- Com símbolo: `€ 40,80` ou `$ 40.80`
- Sem símbolo: `40,80` ou `40.80`
- Com separador: `1.234,56` ou `1,234.56`

### **Quartos:**
- Formato: `Q 3 - Cama 01` → Quarto 3
- Formato: `Quarto 103` → Quarto 103
- Número direto: `5` → Quarto 5

---

## 💡 Dicas e Boas Práticas

### **Antes de Importar:**

1. **Faça backup** dos dados atuais
2. **Teste com arquivo pequeno** (5-10 linhas)
3. **Verifique formato** das datas
4. **Remova linhas vazias** do arquivo
5. **Verifique cabeçalhos** na primeira linha

### **Durante Importação:**

1. **Leia os erros** cuidadosamente
2. **Corrija arquivo** e reimporte se necessário
3. **Não feche navegador** durante processamento
4. **Aguarde confirmação** antes de sair

### **Após Importar:**

1. **Verifique reservas** na página de Reservas
2. **Confira hóspedes** criados
3. **Valide quartos** adicionados
4. **Ajuste dados** se necessário

---

## ⚠️ Limitações

- **Tamanho máximo:** 10 MB por arquivo
- **Codificação:** UTF-8 recomendado
- **Linhas:** Ilimitadas (dentro do tamanho)
- **Tempo:** Pode levar alguns segundos para arquivos grandes

---

## 🔧 Solução de Problemas

### **Problema: "Arquivo vazio"**
**Solução:** Verifique se salvou corretamente como TSV/CSV

### **Problema: "Data inválida"**
**Solução:** Use formato `dd/MM/yyyy` (ex: 01/01/2026)

### **Problema: "Número insuficiente de colunas"**
**Solução:** Certifique-se de que o arquivo tem todas as colunas necessárias

### **Problema: "Check-out anterior ao check-in"**
**Solução:** Verifique ordem das datas na planilha

### **Problema: "Nome obrigatório"**
**Solução:** Preencha coluna de nome para todas as linhas

---

## 📊 Exemplo de Arquivo TSV

```
Nome	Nascimento	Nº Documento	País	Documento	Cama	Check-in	Check-out	Dias	Valor	Pago	TX Booking	TX pago	Nº reserva	Diaria	Total
Mohamed Tarek	19/12/1995	YZ2PMXCC2	Alemanha	Passaporte	Q 3 - Cama 01	01/01/2026	03/01/2026	2	40,80 €	Online	3,00 €	-	6221128181	20,40 €	43,80 €
Nassine Salam	24/08/1988	YSS034	Tunisia	Passaporte	Q 5 - Cama 02	01/01/2026	03/01/2026	2	-	-	-	-	#DIV/0!	-	- €
```

---

## 📞 Suporte

Se encontrar problemas:

1. Verifique este guia
2. Confira formato do arquivo
3. Teste com arquivo menor
4. Revise mensagens de erro

---

## ✅ Checklist Final

Antes de importar:
- [ ] Arquivo exportado como TSV ou CSV
- [ ] Primeira linha contém cabeçalhos
- [ ] Datas no formato correto
- [ ] Sem linhas vazias
- [ ] Backup dos dados atuais feito
- [ ] Teste com arquivo pequeno realizado

---

**Desenvolvido com ❤️ para facilitar sua gestão hoteleira**
