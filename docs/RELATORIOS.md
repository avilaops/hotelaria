# 📊 Sistema de Relatórios Detalhados

## 🎯 Visão Geral

O sistema de relatórios detalhados permite visualizar, filtrar e exportar todas as informações financeiras e operacionais das reservas em um único local, com 21 colunas de dados completos.

---

## 📋 Campos do Relatório

### Informações do Hóspede
1. **Nome** - Nome completo do hóspede
2. **Nascimento** - Data de nascimento
3. **Nº Documento** - Número do documento de identidade/passaporte
4. **País** - País de origem
5. **Tipo Documento** - Tipo do documento (BI, Passaporte, etc.)

### Informações da Reserva
6. **Cama** - Quarto e cama (ex: "Q 3 - Cama 01")
7. **Check-in** - Data e hora de entrada
8. **Check-out** - Data e hora de saída
9. **Dias** - Número de noites
10. **Pessoas** - Total de hóspedes (adultos + crianças)
11. **Nº Reserva** - Código único da reserva

### Informações Financeiras
12. **Valor** - Valor total da reserva
13. **Comissão** - Valor da comissão
14. **Pago Online** - Se foi pago via Booking/Online (Sim/Não)
15. **Taxa Turismo** - Taxa do Booking ou turismo
16. **Diária Livre Taxa** - Valor da diária sem taxas
17. **Total** - Valor total (comissão + taxa turismo)
18. **Livre Tx** - Valor líquido sem taxas
19. **Diária Fora Paga** - Diária paga fora do sistema

### Informações de Pagamento
20. **Forma Pagamento** - Como foi pago (Dinheiro, Cartão, PIX, etc.)
21. **Data Pagamento** - Quando foi realizado o pagamento

---

## 🔍 Filtros Disponíveis

### Data Início
Filtra reservas a partir de uma data específica de check-in

### Data Fim
Filtra reservas até uma data específica de check-out

### Status
Filtra por status da reserva:
- Todos
- Confirmada
- Check-in Realizado
- Check-out Realizado
- Pendente
- Cancelada

### Nº Reserva
Busca por número específico de reserva

---

## 📊 Estatísticas Exibidas

O painel de estatísticas exibe métricas calculadas automaticamente:

| Métrica | Descrição |
|---------|-----------|
| **Total Reservas** | Número total de reservas no período |
| **Total Hóspedes** | Número de hóspedes únicos |
| **Total Diárias** | Soma de todas as noites |
| **Receita Total** | Soma de todos os valores |
| **Comissões** | Soma de todas as comissões |
| **Taxa Turismo** | Soma de todas as taxas |
| **Receita Líquida** | Valor líquido sem taxas |
| **Média Diária** | Valor médio por noite |

---

## 📥 Exportação para CSV

### Como Exportar
1. Aplique os filtros desejados
2. Clique em **"📥 Exportar CSV"**
3. O arquivo será baixado automaticamente

### Formato do Arquivo
- **Separador:** Ponto-e-vírgula (;)
- **Codificação:** UTF-8
- **Nome:** `relatorio_reservas_YYYYMMDD_HHMMSS.csv`
- **Colunas:** 21 campos completos

### Uso do CSV
O arquivo CSV pode ser aberto em:
- Microsoft Excel
- Google Sheets
- LibreOffice Calc
- Qualquer editor de planilhas

---

## 🎨 Interface e Design

### Cabeçalho
- Gradiente azul (igual ao calendário)
- Colunas fixas ao rolar
- Fonte otimizada para legibilidade

### Cores e Badges
- **Verde** - Valores positivos (receita líquida)
- **Vermelho** - Valores negativos (comissões)
- **Azul** - Números de reserva
- **Laranja** - Formas de pagamento
- **Cinza** - Pago presencial

### Responsividade
- Desktop: Tabela completa com scroll horizontal
- Tablet: Colunas reduzidas
- Mobile: Fonte menor, layout adaptado

---

## 💾 Armazenamento de Dados

### Modelo de Dados Atualizado

O modelo `Reserva` foi expandido com os seguintes campos:

```csharp
// Campos Financeiros Detalhados
public decimal ValorTotal { get; set; }
public decimal Comissao { get; set; }
public decimal TaxaTurismo { get; set; }
public decimal DiariaLivreTaxa { get; set; }
public decimal ValorComissaoMaisTaxa { get; set; }
public decimal LivreTx { get; set; }
public decimal DiariaForaPaga { get; set; }

// Informações de Pagamento
public TipoPagamento TipoPagamento { get; set; }
public FormaPagamento FormaPagamento { get; set; }
public DateTime? DataPagamento { get; set; }
public bool PagoOnline { get; set; }

// Informações Redundantes (para relatório)
public string? NumeroDocumentoHospede { get; set; }
public DateTime? DataNascimentoHospede { get; set; }
public string? PaisHospede { get; set; }
public string? TipoDocumentoHospede { get; set; }
public string? NumeroQuarto { get; set; }
public string? TipoCama { get; set; }
```

### Enumeração de Formas de Pagamento

```csharp
public enum FormaPagamento
{
    Dinheiro,
    CartaoCredito,
    CartaoDebito,
    TransferenciaBancaria,
    PIX,
    Online,
    MBWay,
    Multibanco
}
```

---

## 🔄 Integração com Importação

O sistema de importação foi atualizado para preencher automaticamente todos os campos:

### Campos Calculados Automaticamente
- **Diária Livre Taxa** - Calculada a partir da diária e taxas
- **Valor Comissão + Taxa** - Soma automática
- **Livre Tx** - Total menos taxas e comissões
- **Pago Online** - Detectado pelo tipo de pagamento

### Mapeamento Inteligente
- **Forma de Pagamento** - Detecta palavras-chave (dinheiro, cartão, PIX, etc.)
- **País** - Extraído do CSV
- **Tipo Documento** - Extraído do CSV

---

## 🚀 Casos de Uso

### 1. Análise Mensal
```
1. Definir Data Início: 01/01/2026
2. Definir Data Fim: 31/01/2026
3. Clicar em "Buscar"
4. Visualizar estatísticas do mês
5. Exportar para análise externa
```

### 2. Auditoria Financeira
```
1. Filtrar por Status: "Check-out Realizado"
2. Ordenar por Data Pagamento
3. Verificar formas de pagamento
4. Exportar para contabilidade
```

### 3. Relatório de Comissões
```
1. Filtrar período desejado
2. Analisar coluna "Comissão"
3. Verificar "Taxa Turismo"
4. Calcular totais na estatística
5. Exportar para relatório
```

### 4. Análise de Hóspedes
```
1. Buscar por Nº Reserva específico
2. Ver dados completos do hóspede
3. Verificar histórico de pagamentos
4. Analisar padrões de reserva
```

---

## 📱 Acesso

### Navegação
- Menu lateral → **📊 Relatórios**
- URL direta: `/relatorios`

### Permissões
Atualmente acessível para todos os usuários.  
Futura implementação: controle de acesso por perfil.

---

## 💡 Dicas e Boas Práticas

### Filtros
1. ✅ Use filtros de data para análises periódicas
2. ✅ Combine filtros para relatórios específicos
3. ✅ Limpe filtros para ver dados completos

### Exportação
1. ✅ Exporte regularmente para backup
2. ✅ Use CSV para análises externas
3. ✅ Mantenha nomenclatura de arquivos organizada

### Análise
1. ✅ Verifique estatísticas antes de exportar
2. ✅ Cruze dados com relatórios financeiros
3. ✅ Monitore formas de pagamento

---

## 🔧 Funcionalidades Técnicas

### Serviço de Relatório
Classe: `RelatorioService`

**Métodos Principais:**
- `ObterReservasDetalhadas()` - Lista completa com filtros
- `ObterEstatisticas()` - Cálculo de métricas
- `ExportarParaCSV()` - Geração de arquivo

### Performance
- Filtragem eficiente em memória
- Ordenação por data de check-in (desc)
- Scroll virtual para grandes volumes

### Segurança
- Validação de datas
- Sanitização de dados para CSV
- Encoding UTF-8 garantido

---

## 📊 Exemplo de Uso

### Relatório de Janeiro 2026

**Filtros Aplicados:**
- Data Início: 01/01/2026
- Data Fim: 31/01/2026
- Status: Todos

**Resultado:**
```
Total Reservas: 45
Total Hóspedes: 38
Total Diárias: 180
Receita Total: € 3.654,00
Comissões: € 547,50
Taxa Turismo: € 135,00
Receita Líquida: € 2.971,50
Média Diária: € 20,30
```

**Exportado:**
`relatorio_reservas_20260131_153045.csv` (45 linhas)

---

## 🎓 Comparação com Outras Páginas

| Página | Foco | Dados |
|--------|------|-------|
| **Dashboard** | Visão geral | Resumo executivo |
| **Reservas** | Gestão operacional | Dados básicos |
| **Financeiro** | Análise financeira | Receitas e ocupação |
| **Relatórios** | Dados completos | 21 campos detalhados ✅ |

---

## 🚀 Melhorias Futuras

### Curto Prazo
1. ✅ Filtros por forma de pagamento
2. ✅ Ordenação por coluna
3. ✅ Paginação para grandes volumes
4. ✅ Gráficos interativos

### Médio Prazo
1. 🔄 Exportação para Excel (.xlsx)
2. 🔄 Exportação para PDF
3. 🔄 Agendamento de relatórios
4. 🔄 Templates personalizados

### Longo Prazo
1. 📅 Relatórios comparativos (mês vs mês)
2. 📊 Dashboard de BI integrado
3. 🤖 Análise preditiva de receitas
4. 🌐 API de relatórios para integrações

---

## ✅ Checklist de Funcionalidades

### Implementado
- [x] Tabela com 21 colunas
- [x] Filtros por data, status e reserva
- [x] Estatísticas calculadas
- [x] Exportação para CSV
- [x] Design responsivo
- [x] Badges coloridos
- [x] Scroll horizontal/vertical
- [x] Integração com importação
- [x] Link no menu lateral

### Pendente
- [ ] Ordenação por coluna
- [ ] Paginação
- [ ] Gráficos
- [ ] Exportação Excel/PDF
- [ ] Filtros avançados

---

**🎉 Sistema de Relatórios Completo e Funcional**

*Versão: 2.2 - Janeiro 2026*
