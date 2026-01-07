# 📥 Guia de Importação de Dados

## 🎯 Visão Geral

O sistema de importação permite que você importe múltiplas reservas de uma vez a partir de arquivos CSV, TSV ou TXT (Excel), incluindo:
- Informações dos hóspedes
- Dados das reservas
- Detalhes financeiros
- Criação automática de quartos e hóspedes

### ✨ Recursos Principais

- ✅ **Detecção automática** de separador (vírgula, ponto-e-vírgula ou tabulação)
- ✅ **Validação inteligente** com avisos e erros diferenciados
- ✅ **Preview completo** dos dados antes de importar
- ✅ **Criação automática** de hóspedes e quartos
- ✅ **Suporte a múltiplos formatos** de data e valores
- ✅ **Feedback visual** detalhado do processo

---

## 📋 Preparação do Arquivo

### **Passo 1: Exportar do Excel**

1. Abra sua planilha no Excel
2. Selecione **Arquivo** → **Salvar Como**
3. Em **Tipo de arquivo**, escolha:
   - **Texto (Separado por Tabulações) (*.txt)** OU
   - **CSV (Separado por vírgulas) (*.csv)** OU
   - **CSV UTF-8 (Separado por vírgulas) (*.csv)**
4. Clique em **Salvar**

### **Passo 2: Verificar Formato**

Seu arquivo deve ter a seguinte estrutura de colunas:

| Coluna | Descrição | Obrigatório | Exemplo |
|--------|-----------|-------------|---------|
| Nome | Nome completo do hóspede | ✅ Sim | Mohamed Tarek Ibrahim |
| Nascimento | Data de nascimento | ⚠️ Opcional | 19/12/1995 |
| Nº Documento | Número do documento | ⚠️ Recomendado | YZ2PMXCC2 |
| País | País de origem | ⚠️ Opcional | Alemanha |
| Tipo Doc | Tipo (Id/Passaporte) | ⚠️ Opcional | Passaporte |
| Cama | Quarto e cama | ✅ Sim | Q 3 - Cama 01 |
| Check-in | Data de entrada | ✅ Sim | 01/01/2026 |
| Check-out | Data de saída | ✅ Sim | 03/01/2026 |
| Dias Pessoas | Número | ⚠️ Opcional | 2 |
| Valor Pago | Valor em € | ⚠️ Opcional | 40,80 € |
| Tipo Pago | Tipo (Online/Cartão) | ⚠️ Opcional | Online |
| TX Booking | Taxa booking | ⚠️ Opcional | 3,00 € |
| TX Pago | Taxa pagamento | ⚠️ Opcional | - |
| Nº Reserva | Código único | ✅ Sim | 6221128181 |
| Diária | Valor diária | ⚠️ Recomendado | 20,40 € |
| Total | Valor total | ✅ Sim | 43,80 € |

---

## 🚀 Processo de Importação

### **Passo 1: Acessar Página de Importação**

1. Acesse: `http://localhost:5000/importar`
2. Ou clique em **📥 Importar Dados** no menu lateral

### **Passo 2: Selecionar Arquivo**

1. Clique na área de upload ou arraste o arquivo
2. Selecione seu arquivo `.txt`, `.csv` ou `.tsv`
3. O sistema detectará automaticamente o separador e processará

### **Passo 3: Revisar Resultado**

O sistema mostrará estatísticas detalhadas:

```
✅ 45 Linhas Válidas     (95.7% do total)
❌ 2 Com Erros           (4.3% do total)
📊 47 Total de Linhas
💰 € 1.234,56 Valor Total
```

### **Tipos de Mensagens:**

- **❌ Erro:** Linha não será importada (dados essenciais faltando)
- **⚠️ Aviso:** Linha será importada, mas com dados incompletos

**Erros Comuns:**
- ❌ Nome obrigatório não preenchido
- ❌ Data de check-in inválida ou não informada
- ❌ Data de check-out inválida ou não informada
- ❌ Check-out anterior ao check-in
- ❌ Número de reserva obrigatório não informado

**Avisos Comuns:**
- ⚠️ Documento não informado
- ⚠️ Valor total da reserva é zero
- ⚠️ Quarto não identificado (atribuído quarto padrão)
- ⚠️ Check-in muito antigo
- ⚠️ Check-out muito distante

### **Passo 4: Preview dos Dados**

Revise as primeiras 5 linhas válidas:

| Nome | Documento | País | Check-in | Check-out | Noites | Quarto | Diária | Total | Status |
|------|-----------|------|----------|-----------|--------|--------|--------|-------|--------|
| Mohamed... | YZ2PMXCC2 | 🌍 Alemanha | 01/01/2026 | 03/01/2026 | 2 | Q 3 | € 20,40 | € 43,80 | ✓ Válido |

### **Passo 5: Confirmar Importação**

1. Revise os dados no preview
2. Verifique se há erros críticos
3. Clique em **"✅ Importar X Reservas"**
4. Aguarde o processamento (indicador visual aparecerá)
5. Veja mensagem de sucesso
6. Clique em **"📋 Ver Reservas Importadas"** para visualizar

---

## 🔍 Validações Automáticas

### **Validações de Dados Obrigatórios:**
- ✅ Nome do hóspede
- ✅ Data de check-in válida
- ✅ Data de check-out válida
- ✅ Número de reserva único

### **Validações de Regras de Negócio:**
- ✅ Check-out deve ser posterior ao check-in
- ✅ Datas em formato válido (múltiplos formatos suportados)
- ✅ Valores numéricos não negativos
- ✅ Datas dentro de período razoável (±2 anos)

### **Validações de Integridade:**
- ✅ Número mínimo de colunas (16 esperadas)
- ✅ Cabeçalho válido na primeira linha
- ✅ Dados consistentes entre colunas

### **Criação Automática:**
- 🆕 **Hóspedes novos** criados se documento não existir
  - Email temporário: `nome@importado.com`
  - Telefone: vazio (preencher depois)
- 🆕 **Quartos novos** criados se número não existir
  - Tipo: Standard
  - Capacidade: 2 pessoas
  - Comodidades padrão: Wi-Fi, TV, Ar condicionado
- 🆕 **Reservas** com status baseado na data
  - Se check-in já passou: "Check-in Realizado"
  - Se check-in futuro: "Confirmada"

---

## 🎨 Formatos Suportados

### **Separadores:**
- Tabulação (`\t`) - TSV
- Vírgula (`,`) - CSV
- Ponto-e-vírgula (`;`) - CSV europeu
- **Detecção automática** do separador

### **Datas:**
- `dd/MM/yyyy` → 01/01/2026
- `dd-MM-yyyy` → 01-01-2026
- `yyyy-MM-dd` → 2026-01-01
- `MM/dd/yyyy` → 01/01/2026

### **Valores Monetários:**
- Com símbolo: `€ 40,80`, `$ 40.80`, `R$ 40,80`
- Sem símbolo: `40,80`, `40.80`
- Com separador de milhares: `1.234,56`, `1,234.56`
- Negativos: `-40,80`, `(40.80)`

### **Identificação de Quartos:**
- `Q 3 - Cama 01` → Quarto 3
- `Quarto 103` → Quarto 103
- `Q103` → Quarto 103
- `5` → Quarto 5 (número direto)

---

## 💡 Dicas e Boas Práticas

### **Antes de Importar:**

1. ✅ **Faça backup** dos dados atuais do sistema
2. ✅ **Teste com arquivo pequeno** (5-10 linhas primeiro)
3. ✅ **Verifique formato** das datas (preferencialmente dd/MM/yyyy)
4. ✅ **Remova linhas vazias** do final do arquivo
5. ✅ **Mantenha cabeçalhos** na primeira linha
6. ✅ **Use UTF-8** para caracteres especiais (ã, é, ç, etc.)
7. ✅ **Numere reservas** de forma única

### **Durante Importação:**

1. 👀 **Leia os erros e avisos** cuidadosamente
2. 📝 **Corrija arquivo** se houver muitos erros
3. ⏳ **Não feche navegador** durante processamento
4. ✅ **Revise preview** antes de confirmar
5. 💾 **Aguarde confirmação** de sucesso

### **Após Importar:**

1. 🔍 **Verifique reservas** na página de Reservas
2. 👥 **Confira hóspedes** criados automaticamente
3. 🛏️ **Valide quartos** adicionados
4. ✏️ **Complete dados faltantes** (emails, telefones)
5. 💰 **Revise valores** e comissões

---

## ⚠️ Limitações e Requisitos

### **Limitações Técnicas:**
- **Tamanho máximo:** 10 MB por arquivo
- **Codificação:** UTF-8 recomendado (para acentos)
- **Linhas:** Ilimitadas (dentro do tamanho)
- **Tempo de processamento:** Varia com tamanho do arquivo

### **Requisitos do Arquivo:**
- Primeira linha deve conter cabeçalhos
- Mínimo de 16 colunas (até 20 suportadas)
- Formato de texto (não Excel .xlsx)
- Separador consistente em todo arquivo

---

## 🔧 Solução de Problemas

### **"Arquivo vazio"**
- ✅ Verifique se salvou corretamente como TSV/CSV
- ✅ Confirme que arquivo tem conteúdo
- ✅ Tente abrir no Bloco de Notas para verificar

### **"Data inválida"**
- ✅ Use formato `dd/MM/yyyy` (ex: 01/01/2026)
- ✅ Verifique se não há espaços extras
- ✅ Confirme que dia/mês/ano são válidos

### **"Número insuficiente de colunas"**
- ✅ Certifique-se de ter todas as 16+ colunas
- ✅ Verifique se não faltam separadores
- ✅ Confirme que separador está correto

### **"Check-out anterior ao check-in"**
- ✅ Verifique ordem das datas na planilha
- ✅ Confirme colunas Check-in e Check-out
- ✅ Valide datas no Excel antes de exportar

### **"Nome obrigatório"**
- ✅ Preencha coluna de nome para todas as linhas
- ✅ Remova linhas vazias
- ✅ Verifique se não há células em branco

### **"Quarto não identificado"**
- ✅ Use formato `Q 3` ou `Quarto 3`
- ✅ Sistema atribuirá quarto 1 como padrão
- ✅ Corrija manualmente após importação se necessário

### **Caracteres estranhos (�, ã vira Ã)**
- ✅ Salve arquivo como "CSV UTF-8" no Excel
- ✅ Use Bloco de Notas para salvar com UTF-8
- ✅ No Excel: Arquivo → Salvar Como → CSV UTF-8

---

## 📊 Exemplos de Arquivos

### **Exemplo TSV (Tab-separated):**

```tsv
Nome	Nascimento	Nº Documento	País	Tipo Doc	Cama	Check-in	Check-out	Dias	Valor	Pago	TX Booking	TX pago	Nº reserva	Diaria	Total
Mohamed Tarek	19/12/1995	YZ2PMXCC2	Alemanha	Passaporte	Q 3 - Cama 01	01/01/2026	03/01/2026	2	40,80 €	Online	3,00 €	-	6221128181	20,40 €	43,80 €
Nassine Salam	24/08/1988	YSS034	Tunisia	Passaporte	Q 5 - Cama 02	01/01/2026	05/01/2026	4	-	Cartão	-	-	6221128182	18,00 €	72,00 €
```

### **Exemplo CSV (Comma-separated):**

```csv
Nome,Nascimento,Nº Documento,País,Tipo Doc,Cama,Check-in,Check-out,Dias,Valor,Pago,TX Booking,TX pago,Nº reserva,Diaria,Total
"Mohamed Tarek",19/12/1995,YZ2PMXCC2,Alemanha,Passaporte,"Q 3 - Cama 01",01/01/2026,03/01/2026,2,"40,80 €",Online,"3,00 €",-,6221128181,"20,40 €","43,80 €"
```

---

## 📞 Suporte e Ajuda

Se encontrar problemas:

1. 📖 Consulte este guia completo
2. 🔍 Confira mensagens de erro detalhadas
3. 🧪 Teste com arquivo pequeno primeiro
4. 📝 Verifique formato do arquivo
5. 💬 Revise exemplos fornecidos

### **Recursos Adicionais:**
- Link da documentação na página de importação
- Preview antes de importar
- Mensagens de erro detalhadas
- Estatísticas em tempo real

---

## ✅ Checklist Final

Antes de importar:
- [ ] Arquivo exportado como TSV, CSV ou TXT
- [ ] Primeira linha contém cabeçalhos corretos
- [ ] Datas no formato dd/MM/yyyy
- [ ] Sem linhas completamente vazias
- [ ] Números de reserva únicos
- [ ] Backup dos dados atuais feito
- [ ] Teste com arquivo pequeno realizado (5-10 linhas)
- [ ] Arquivo salvo com codificação UTF-8
- [ ] Valores monetários formatados corretamente

Durante importação:
- [ ] Revisar estatísticas apresentadas
- [ ] Ler todos os erros e avisos
- [ ] Conferir preview dos dados
- [ ] Confirmar que dados estão corretos
- [ ] Aguardar processamento completo

Após importação:
- [ ] Verificar reservas na lista
- [ ] Conferir hóspedes criados
- [ ] Validar quartos adicionados
- [ ] Completar dados faltantes
- [ ] Revisar valores e comissões

---

**🎉 Desenvolvido com ❤️ para facilitar sua gestão hoteleira**

*Última atualização: Janeiro 2026*
