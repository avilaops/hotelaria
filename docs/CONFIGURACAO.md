# ⚙️ Módulo de Configuração

## 🎯 Visão Geral

O módulo de configuração centraliza todas as configurações e ajustes do sistema em uma única interface intuitiva e organizada. Acessível através de um botão fixo no rodapé do menu lateral.

---

## 🎨 Localização

### Botão no Menu
```
┌─────────────────┐
│ 🏨 Hotelaria    │
├─────────────────┤
│ 🏠 Página Princ │
│ 📋 Reservas     │
│ 👥 Hóspedes     │
│ 📅 Disponib...  │
│ 💰 Financeiro   │
│ 📊 Relatórios   │
│                 │
│                 │
├─────────────────┤
│ ⚙️ Configurações│ ← Canto inferior esquerdo
└─────────────────┘
```

---

## 📋 Seções do Módulo

### 1. **Gestão de Dados** 🏢

Gerenciamento de dados básicos do sistema.

#### Cards Disponíveis

##### ✅ Quartos
- **Status:** Ativo
- **Função:** Gerenciar quartos, vagas e preços
- **Acesso:** Click no card → Redireciona para `/quartos`

##### 🔜 Preços e Taxas
- **Status:** Em breve
- **Função:** Configurar preços dinâmicos e taxas
- **Preview:** Não clicável

##### 🔜 Políticas
- **Status:** Em breve
- **Função:** Definir políticas de cancelamento e check-in
- **Preview:** Não clicável

### 2. **Sistema** ⚙️

Configurações gerais do sistema.

#### Cards Disponíveis

##### 🔜 Usuários e Permissões
- **Status:** Em breve
- **Função:** Gerenciar usuários e níveis de acesso
- **Preview:** Sistema multi-usuário

##### 🔜 Idioma e Região
- **Status:** Em breve
- **Função:** Configurar idioma, moeda e fuso horário
- **Preview:** Internacionalização

##### 🔜 Notificações
- **Status:** Em breve
- **Função:** Configurar alertas e notificações
- **Preview:** Sistema de alertas

### 3. **Integrações** 🔗

Conectividade com serviços externos.

#### Cards Disponíveis

##### 🔜 Booking.com
- **Status:** Em breve
- **Função:** Sincronizar com Booking.com
- **Preview:** API de integração

##### 🔜 Airbnb
- **Status:** Em breve
- **Função:** Conectar com conta Airbnb
- **Preview:** Sincronização bidirecional

##### 🔜 E-mail
- **Status:** Em breve
- **Função:** Configurar envio automático de e-mails
- **Preview:** Templates personalizados

### 4. **Dados e Backup** 💾

Gerenciamento de dados e backups.

#### Cards Disponíveis

##### ✅ Importar Dados
- **Status:** Ativo
- **Função:** Importar reservas de CSV/TSV
- **Acesso:** Click no card → Redireciona para `/importar`

##### 🔜 Exportar Dados
- **Status:** Em breve
- **Função:** Exportar todos os dados do sistema
- **Preview:** Formato JSON/CSV

##### 🔜 Backup Automático
- **Status:** Em breve
- **Função:** Configurar backups periódicos
- **Preview:** Agendamento automático

### 5. **Informações do Sistema** ℹ️

Detalhes técnicos e versão.

#### Informações Exibidas
- **Versão:** 2.3.0
- **Ambiente:** Desenvolvimento
- **Tecnologia:** Blazor Server (.NET 8)
- **Última Atualização:** 07/01/2026

---

## 🎨 Design

### Estrutura Visual

```
┌─────────────────────────────────────────────────┐
│ ⚙️ Configurações                                │
├─────────────────────────────────────────────────┤
│                                                 │
│ ┌─────────────────────────────────────────┐   │
│ │ 🏢  Gestão de Dados                     │   │
│ │     Configure e gerencie os dados       │   │
│ ├─────────────────────────────────────────┤   │
│ │ ┌────────┬────────┬────────┐            │   │
│ │ │🛏️Quartos│💰Preços│📋Políti│            │   │
│ │ │  (ativo)│(breve) │(breve) │            │   │
│ │ └────────┴────────┴────────┘            │   │
│ └─────────────────────────────────────────┘   │
│                                                 │
│ ┌─────────────────────────────────────────┐   │
│ │ ⚙️  Sistema                             │   │
│ │     Configurações gerais                │   │
│ ├─────────────────────────────────────────┤   │
│ │ [Cards...]                              │   │
│ └─────────────────────────────────────────┘   │
│                                                 │
└─────────────────────────────────────────────────┘
```

### Cores e Estados

| Elemento | Estado | Visual |
|----------|--------|--------|
| Card Ativo | Clicável | Hover + Sombra + Borda azul |
| Card Desabilitado | Não clicável | Opacidade 60% + Badge "Em breve" |
| Ícone de Seção | Gradiente | Fundo roxo (#667eea → #764ba2) |
| Badge Em Breve | Gradiente | Amarelo/Azul (#ffd89b → #19547b) |

### Efeitos de Interação

#### Hover em Card Ativo
```css
- Transform: translateY(-4px)
- Box-shadow: Elevação
- Border-color: Azul primário
- Background: Branco
```

#### Hover no Botão Configuração
```css
- Background: rgba(255, 255, 255, 0.1)
- Transform: translateX(4px)
```

---

## 🔧 Funcionalidades Técnicas

### Navegação Programática

```csharp
@inject NavigationManager NavigationManager

// Click no card
<div class="config-card" @onclick='() => NavigationManager.NavigateTo("/quartos")'>
    ...
</div>
```

### Cards Desabilitados

```html
<div class="config-card disabled">
    <div class="card-icon">💰</div>
    <div class="card-content">
        <h3>Preços e Taxas</h3>
        <p>Configure preços dinâmicos e taxas</p>
        <span class="badge-soon">Em breve</span>
    </div>
    <div class="card-arrow">→</div>
</div>
```

### Estrutura de Seção

```html
<div class="config-section">
    <div class="section-header">
        <div class="section-icon">🏢</div>
        <div class="section-info">
            <h2>Gestão de Dados</h2>
            <p>Configure e gerencie os dados básicos</p>
        </div>
    </div>
    <div class="config-cards">
        <!-- Cards aqui -->
    </div>
</div>
```

---

## 📱 Responsividade

### Desktop (> 1024px)
- Cards em grid 3 colunas
- Seções com espaçamento amplo
- Ícones grandes

### Tablet (768px - 1024px)
- Cards em grid 2 colunas
- Ícones médios
- Espaçamento reduzido

### Mobile (< 768px)
- Cards em coluna única
- Botão de config mostra apenas ícone ⚙️
- Seções com padding reduzido
- Header de seção em coluna

---

## 🚀 Casos de Uso

### 1. Acessar Gestão de Quartos
```
1. Clicar no botão "⚙️ Configurações" no rodapé do menu
2. Navegar até seção "Gestão de Dados"
3. Clicar no card "🛏️ Quartos"
4. Sistema redireciona para página de quartos
```

### 2. Ver Funcionalidades Futuras
```
1. Acessar configurações
2. Percorrer as seções
3. Cards com badge "Em breve" mostram funcionalidades planejadas
4. Não é possível clicar nestes cards
```

### 3. Importar Dados
```
1. Acessar configurações
2. Rolar até "Dados e Backup"
3. Clicar em "📥 Importar Dados"
4. Sistema redireciona para página de importação
```

### 4. Verificar Versão do Sistema
```
1. Acessar configurações
2. Rolar até "Informações do Sistema"
3. Ver versão, ambiente e tecnologia
```

---

## 📊 Estatísticas

| Métrica | Valor |
|---------|-------|
| Total de Seções | 5 |
| Cards Ativos | 2 (Quartos, Importar) |
| Cards Futuros | 11 |
| Total de Cards | 13 |
| Ícones Usados | 18 emojis únicos |

---

## 💡 Melhorias Futuras

### Curto Prazo
1. ✅ Sistema de busca na configuração
2. ✅ Favoritos rápidos
3. ✅ Histórico de alterações
4. ✅ Atalhos de teclado

### Médio Prazo
1. 🔄 Implementar cards desabilitados
   - Usuários e Permissões
   - Idioma e Região
   - Notificações
2. 🔄 Preços dinâmicos
3. 🔄 Políticas personalizadas
4. 🔄 Sistema de backup

### Longo Prazo
1. 📅 Integração Booking.com
2. 📅 Integração Airbnb
3. 📅 E-mail automatizado
4. 📅 API pública
5. 📅 Plugins de terceiros

---

## 🎓 Boas Práticas

### Organização
1. ✅ Agrupe funcionalidades relacionadas
2. ✅ Use ícones intuitivos
3. ✅ Mantenha descrições claras
4. ✅ Indique status (ativo/em breve)

### Usabilidade
1. ✅ Botão de fácil acesso (rodapé fixo)
2. ✅ Navegação rápida
3. ✅ Feedback visual ao hover
4. ✅ Cards clicáveis bem sinalizados

### Desenvolvimento
1. ✅ Estrutura modular
2. ✅ Fácil adicionar novos cards
3. ✅ CSS bem organizado
4. ✅ Responsivo por padrão

---

## 📝 Checklist de Implementação

### Funcionalidades
- [x] Página de configuração criada
- [x] Botão no rodapé do menu
- [x] Emoji ⚙️ no botão
- [x] 5 seções organizadas
- [x] 13 cards (2 ativos, 11 futuros)
- [x] Navegação para Quartos
- [x] Navegação para Importar
- [x] Badges "Em breve"
- [x] Informações do sistema

### Design
- [x] Layout responsivo
- [x] Gradientes em ícones
- [x] Hover effects
- [x] Animações de entrada
- [x] Cards desabilitados visualmente
- [x] Grid de informações

### Documentação
- [x] Guia completo
- [x] Casos de uso
- [x] Screenshots visuais
- [x] Melhorias futuras

---

## 🎉 Resultado

O módulo de configuração oferece:
- ✅ **Centralização** de todas as configurações
- ✅ **Interface intuitiva** e organizada
- ✅ **Fácil acesso** via botão fixo
- ✅ **Escalabilidade** para futuras funcionalidades
- ✅ **Visual moderno** com animações
- ✅ **Responsivo** em todos os dispositivos

---

**🎉 Módulo de Configuração Completo e Funcional**

*Versão: 2.4 - Janeiro 2026*
*Centralizando o controle do sistema*
