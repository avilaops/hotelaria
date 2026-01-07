# 🛏️ Sistema de Vagas no Calendário de Ocupação

## 🎯 Visão Geral

O módulo de disponibilidade foi completamente reformulado para suportar múltiplas vagas por quarto, ideal para hostels, albergues e dormitórios compartilhados. Agora cada quarto pode ter várias vagas/camas independentes.

---

## ✨ Principais Melhorias

### 1. **Múltiplas Vagas por Quarto** 🛏️

Cada quarto agora possui a propriedade `NumeroVagas` que define quantas camas/vagas disponíveis existem.

#### Configuração Atual
| Quarto | Vagas | Tipo | Preço/Noite |
|--------|-------|------|-------------|
| Quarto 1 | 8 vagas | Standard | € 20,00 |
| Quarto 2 | 6 vagas | Standard | € 20,00 |
| Quarto 3 | 4 vagas | Standard | € 25,00 |
| Quarto 4 | 3 vagas | Deluxe | € 30,00 |
| Quarto 5 | 2 vagas | Suite | € 50,00 |

**Total:** 23 vagas disponíveis

### 2. **Visualização no Calendário** 📅

O calendário agora exibe **uma linha por vaga**:

```
┌─────────────┬────┬────┬────┬────┐
│ Quarto 1    │ 01 │ 02 │ 03 │ 04 │
│ Standard    │    │    │    │    │
│ 🛏️ 8 vagas  │    │    │    │    │
├─────────────┼────┼────┼────┼────┤
│ (linha 1)   │ □  │ □  │ □  │ □  │
│ (linha 2)   │ □  │ ■  │ ■  │ □  │
│ (linha 3)   │ □  │ □  │ □  │ □  │
│ (linha 4)   │ □  │ □  │ ■  │ ■  │
│ (linha 5)   │ □  │ □  │ □  │ □  │
│ (linha 6)   │ □  │ □  │ □  │ □  │
│ (linha 7)   │ □  │ □  │ □  │ □  │
│ (linha 8)   │ □  │ □  │ □  │ □  │
└─────────────┴────┴────┴────┴────┘
```

**Legenda:**
- □ = Vaga disponível
- ■ = Vaga ocupada

### 3. **Interatividade Aprimorada** 🖱️

#### Click em Célula Disponível
- Abre modal para **criar nova reserva**
- Pré-preenche data e quarto
- Permite selecionar hóspede
- Define check-in e check-out
- Atribui vaga automaticamente

#### Click em Célula Ocupada
- Abre modal para **editar reserva existente**
- Permite alterar datas de check-in/out
- Permite mudar status
- Permite adicionar observações
- Opção de excluir reserva

### 4. **Modal de Reserva** 📝

#### Campos do Modal
1. **Quarto** - Seleção do quarto (desabilitado ao editar)
2. **Vaga** - Número da vaga/cama (1 a N)
3. **Check-in** - Data e hora de entrada
4. **Check-out** - Data e hora de saída
5. **Hóspede** - Seleção do hóspede cadastrado
6. **Valor Total** - Valor da reserva
7. **Status** - Status da reserva (Confirmada, Pendente, etc.)
8. **Observações** - Campo livre para anotações

#### Ações Disponíveis
- **💾 Salvar** - Criar ou atualizar reserva
- **🗑️ Excluir** - Remover reserva (apenas ao editar)
- **Cancelar** - Fechar sem salvar

### 5. **Busca de Disponibilidade** 🔍

A busca agora considera **vagas disponíveis** por quarto:

```
Buscar Quartos Disponíveis
├── Data Check-in: 08/01/2026
├── Data Check-out: 09/01/2026
├── Nº de Hóspedes: 2
└── 🔍 Buscar

Resultado:
✓ Quarto 1 - 6 de 8 vagas disponíveis
✓ Quarto 2 - 5 de 6 vagas disponíveis
✓ Quarto 3 - 4 de 4 vagas disponíveis
```

### 6. **Estatísticas Atualizadas** 📊

#### Painel de Estatísticas
- **Taxa de Ocupação** - Baseado em vagas ocupadas
- **Vagas Disponíveis** - Soma de todas as vagas livres
- **Check-ins Hoje** - Número de check-ins do dia
- **Check-outs Hoje** - Número de check-outs do dia

#### Cálculo de Ocupação
```
Total de Vagas: 23
Vagas Ocupadas: 12
Vagas Disponíveis: 11
Taxa de Ocupação: 52%
```

---

## 🔧 Implementação Técnica

### Modelo de Dados

#### Atualização em `Quarto.cs`
```csharp
public class Quarto
{
    public int Id { get; set; }
    public string Numero { get; set; }
    public TipoQuarto Tipo { get; set; }
    public int Capacidade { get; set; }
    public int NumeroVagas { get; set; } = 1; // NOVO
    public decimal PrecoPorNoite { get; set; }
    // ... outros campos
}
```

### Lógica de Vagas

#### Associação Reserva-Vaga
As reservas são associadas a vagas específicas através do campo `Observacoes`:
```csharp
reserva.Observacoes = "... | Vaga:3";
```

#### Verificação de Disponibilidade
```csharp
private Reserva? ObterReservaDiaVaga(int quartoId, DateTime data, int vaga)
{
    return reservasMes.FirstOrDefault(r => 
        r.QuartoId == quartoId && 
        r.CheckIn.Date <= data && 
        r.CheckOut.Date > data &&
        (r.Observacoes?.Contains($"Vaga:{vaga}") ?? false));
}
```

#### Contagem de Vagas Disponíveis
```csharp
private int GetVagasDisponiveisQuarto(int quartoId, DateTime checkIn, DateTime checkOut)
{
    var quarto = QuartoService.ObterPorId(quartoId);
    var vagasOcupadas = ReservaService.ObterTodas()
        .Count(r => r.QuartoId == quartoId && 
                   r.CheckIn < checkOut && 
                   r.CheckOut > checkIn &&
                   r.Status != StatusReserva.Cancelada);
    
    return quarto.NumeroVagas - vagasOcupadas;
}
```

---

## 🎨 Interface

### Layout do Calendário

```
┌─────────────────────────────────────────────────────┐
│ 📅 Calendário de Ocupação - janeiro 2026           │
│ [◄] [Hoje] [►]                                      │
│ □ Disponível  ■ Reservado  🔑 Check-in  🚪 Check-out│
├─────────────────────────────────────────────────────┤
│ Quarto│Vaga│ 1 │ 2 │ 3 │ 4 │ 5 │ 6 │ 7 │...        │
├───────┼────┼───┼───┼───┼───┼───┼───┼───┼────       │
│   1   │    │   │   │   │   │   │   │   │           │
│Standard│   │   │   │   │   │   │   │   │           │
│🛏️ 8    │ 1  │ □ │ □ │ □ │ □ │ □ │ □ │ □ │           │
│       │ 2  │ □ │🔑IN│ ■ │ ■ │🚪OUT│ □ │ □ │           │
│       │ 3  │ □ │ □ │ □ │ □ │ □ │ □ │ □ │           │
│       │ 4  │ □ │ □ │ □ │ □ │ □ │ □ │ □ │           │
│       │ 5  │ □ │ □ │ □ │ □ │ □ │ □ │ □ │           │
│       │ 6  │ □ │ □ │ □ │ □ │ □ │ □ │ □ │           │
│       │ 7  │ □ │ □ │ □ │ □ │ □ │ □ │ □ │           │
│       │ 8  │ □ │ □ │ □ │ □ │ □ │ □ │ □ │           │
└───────┴────┴───┴───┴───┴───┴───┴───┴───┴────       │
```

### Cores e Estados

| Estado | Cor | Visual |
|--------|-----|--------|
| Disponível | Verde claro (#e8f5e9) | □ |
| Ocupado | Azul claro (#bbdefb) | ■ |
| Check-in | Verde gradiente | 🔑 IN |
| Check-out | Laranja gradiente | 🚪 OUT |
| Hoje | Amarelo claro | Destaque vertical |

---

## 📱 Casos de Uso

### 1. Reservar uma Vaga
```
1. Navegar até o calendário de ocupação
2. Clicar em uma célula disponível (verde)
3. Modal abre com:
   - Quarto e vaga pré-selecionados
   - Data pré-preenchida
4. Selecionar hóspede
5. Ajustar datas se necessário
6. Definir valor
7. Clicar em "💾 Salvar"
8. Reserva criada e calendário atualiza
```

### 2. Editar Reserva Existente
```
1. Clicar em uma célula ocupada (azul) ou com check-in/out
2. Modal abre com dados da reserva
3. Alterar check-in/check-out
4. Mudar status se necessário
5. Adicionar observações
6. Clicar em "💾 Salvar"
7. Reserva atualizada no calendário
```

### 3. Excluir Reserva
```
1. Clicar na célula da reserva
2. Modal abre
3. Clicar em "🗑️ Excluir"
4. Reserva removida
5. Vaga volta a ficar disponível
```

### 4. Buscar Vagas Disponíveis
```
1. Clicar em "🔍 Buscar Disponibilidade"
2. Definir datas e número de hóspedes
3. Clicar em "🔍 Buscar Quartos"
4. Sistema lista quartos com vagas disponíveis
5. Cada card mostra:
   - Número de vagas disponíveis
   - Preço por noite
   - Total do período
6. Clicar em "➕ Reservar" para criar reserva
```

---

## 💡 Dicas e Boas Práticas

### Gestão de Vagas
1. ✅ Defina número correto de vagas por quarto
2. ✅ Use vagas para dormitórios compartilhados
3. ✅ Mantenha capacidade e vagas coerentes
4. ✅ Revise calendário regularmente

### Reservas
1. ✅ Sempre associe reserva a hóspede cadastrado
2. ✅ Defina valor correto conforme período
3. ✅ Use observações para informações extras
4. ✅ Mantenha status atualizado

### Visualização
1. ✅ Use filtros para período específico
2. ✅ Navegue entre meses com ◄ ►
3. ✅ Use botão "Hoje" para retornar à data atual
4. ✅ Verifique estatísticas para visão geral

---

## 🚀 Melhorias Futuras

### Curto Prazo
1. ✅ Drag & drop para mover reservas entre vagas
2. ✅ Seleção múltipla de vagas para grupo
3. ✅ Cor por hóspede para visualização
4. ✅ Exportar calendário para PDF/Excel

### Médio Prazo
1. 🔄 Histórico de ocupação por vaga
2. 🔄 Preços dinâmicos por vaga
3. 🔄 Bloqueio temporário de vagas
4. 🔄 Notificações de check-in/out

### Longo Prazo
1. 📅 Reservas recorrentes
2. 📊 Analytics por vaga
3. 🤖 Recomendação automática de vagas
4. 🌐 Sincronização com Booking.com

---

## 📊 Comparação Antes vs Depois

### Antes
- ❌ Um quarto = uma linha
- ❌ Sem diferenciação de vagas
- ❌ Reserva por quarto inteiro
- ❌ Não ideal para hostels
- ❌ Sem edição rápida

### Depois
- ✅ Um quarto = múltiplas linhas (uma por vaga)
- ✅ Cada vaga é independente
- ✅ Reserva por vaga específica
- ✅ Perfeito para hostels e dormitórios
- ✅ Edição com um clique
- ✅ Criação rápida de reservas
- ✅ Busca por vagas disponíveis
- ✅ Estatísticas por vaga

---

## ✅ Checklist de Funcionalidades

### Implementado
- [x] Propriedade NumeroVagas no modelo Quarto
- [x] Visualização de múltiplas linhas por quarto
- [x] Uma linha por vaga
- [x] Click para criar nova reserva
- [x] Click para editar reserva existente
- [x] Modal de criação/edição de reserva
- [x] Alteração de datas check-in/out
- [x] Exclusão de reservas
- [x] Busca por vagas disponíveis
- [x] Estatísticas atualizadas
- [x] Tooltips informativos
- [x] Dados de exemplo (5 quartos, 23 vagas total)

### Futuro
- [ ] Drag & drop entre vagas
- [ ] Seleção múltipla para grupos
- [ ] Cores personalizadas por hóspede
- [ ] Histórico por vaga
- [ ] Preços dinâmicos

---

**🎉 Sistema de Vagas Completo e Funcional**

*Versão: 2.3 - Janeiro 2026*
*Ideal para Hostels, Albergues e Dormitórios Compartilhados*
