# 🔧 Guia de Troubleshooting - Módulos Não Aparecem

## 🎯 Problema

Os módulos **Disponibilidade** e **Relatórios** não aparecem no sistema.

---

## 🔍 Causa Raiz

O sistema não está compilando devido a **erros de build**, portanto não inicia e as páginas não carregam.

---

## ✅ Soluções

### Solução 1: Script Automático (Recomendado)

1. **Executar o script:**
```cmd
fix-and-run.bat
```

2. **O script vai:**
   - Limpar cache de build
   - Restaurar dependências
   - Compilar projeto
   - Rodar sistema (se compilar)

---

### Solução 2: Manual (Passo a Passo)

#### Passo 1: Limpar Build

```powershell
Remove-Item -Recurse -Force obj,bin
```

#### Passo 2: Restaurar

```powershell
dotnet restore
```

#### Passo 3: Compilar

```powershell
dotnet build --no-incremental
```

#### Passo 4: Se Compilar com Sucesso

```powershell
dotnet run
```

#### Passo 5: Acessar

```
http://localhost:5000
```

---

### Solução 3: Verificar Erros Específicos

Se a compilação falhar, verifique:

#### Erro 1: Binding Duplicado (Usuarios.razor)

**Sintoma:**
```
error RZ10008: The attribute 'oninput' is used two or more times
```

**Solução:**
Remover `@bind:event="oninput"` da linha 20

#### Erro 2: DataNascimento (RelatorioService.cs)

**Sintoma:**
```
'Hospede' não contém uma definição para "DataNascimento"
```

**Solução:**
Já foi adicionado no modelo. Se persistir, limpar build.

#### Erro 3: Variáveis Inexistentes (Disponibilidade.razor)

**Sintoma:**
```
O nome "hospedeEdicao" não existe no contexto atual
```

**Solução:**
Código antigo/gerado. Limpar build resolve.

---

## 📋 Checklist de Verificação

Após compilar com sucesso:

- [ ] Sistema iniciou sem erros
- [ ] Acessou http://localhost:5000
- [ ] Fez login (admin / admin123)
- [ ] Menu lateral aparece completo
- [ ] Clicou em "📅 Disponibilidade"
- [ ] Página de Disponibilidade carregou
- [ ] Clicou em "📊 Relatórios"
- [ ] Página de Relatórios carregou

---

## 🎯 Verificação Visual do Menu

O menu deve aparecer assim:

```
┌────────────────────────┐
│ 🏨 Hotelaria          │
├────────────────────────┤
│ 🏠 Página Principal    │
│ 📋 Reservas            │
│ 👥 Hóspedes            │
│ 📅 Disponibilidade  ← │
│ 💰 Financeiro          │
│ 📊 Relatórios       ← │
├────────────────────────┤
│ ⚙️ Configurações       │
└────────────────────────┘
```

---

## 🔍 Diagnóstico Avançado

### Verificar se as páginas existem:

```powershell
Test-Path Pages\Disponibilidade.razor
Test-Path Pages\Relatorios.razor
```

Ambos devem retornar **True**.

### Verificar se o roteamento está correto:

As páginas devem ter estas diretivas:

**Disponibilidade.razor:**
```razor
@page "/disponibilidade"
```

**Relatorios.razor:**
```razor
@page "/relatorios"
```

---

## 🚀 Após Correção

1. **Sistema compilou:**
```
✓ Build succeeded
```

2. **Sistema rodando:**
```
Now listening on: http://localhost:5000
```

3. **Páginas acessíveis:**
   - ✅ http://localhost:5000/disponibilidade
   - ✅ http://localhost:5000/relatorios

---

## 💡 Dicas

### Sempre que editar código:

1. **Salvar arquivo** (Ctrl + S)
2. **Aguardar Hot Reload** (automático)
3. **Se não funcionar:**
   - Parar sistema (Ctrl + C)
   - Limpar: `dotnet clean`
   - Recompilar: `dotnet build`
   - Rodar: `dotnet run`

### Visual Studio Code:

```json
// .vscode/tasks.json
{
    "label": "build",
    "command": "dotnet",
    "args": ["build"]
}
```

Pressione: **Ctrl + Shift + B**

---

## 📞 Se Ainda Não Funcionar

### Verificar logs de erro:

```powershell
dotnet build > build-errors.txt 2>&1
```

Abrir `build-errors.txt` e ver erros específicos.

### Verificar versão .NET:

```powershell
dotnet --version
```

Deve ser **8.0.x** ou superior.

### Reinstalar dependências:

```powershell
Remove-Item -Recurse -Force obj,bin
dotnet restore --force
dotnet build
```

---

## 🎉 Sucesso

Quando tudo funcionar, você verá:

```
╔═══════════════════════════════════╗
║  SISTEMA HOTELARIA RODANDO       ║
╠═══════════════════════════════════╣
║  URL:          localhost:5000     ║
║  Status:       ✅ Online          ║
║  Disponibilidade: ✅ Funcionando  ║
║  Relatórios:   ✅ Funcionando     ║
╚═══════════════════════════════════╝
```

---

## 📚 Arquivos Importantes

```
Pages/
├── Disponibilidade.razor  ← Página de disponibilidade
└── Relatorios.razor       ← Página de relatórios

Shared/
└── MainLayout.razor       ← Menu lateral

Models/
├── Hospede.cs            ← Deve ter DataNascimento
├── Quarto.cs
└── Reserva.cs

Services/
├── RelatorioService.cs   ← Usa Hospede.DataNascimento
└── ImportacaoService.cs  ← Também usa
```

---

## ✅ Resumo Executivo

**Problema:** Módulos não aparecem  
**Causa:** Erros de compilação  
**Solução:** Limpar cache + Recompilar  
**Tempo:** 2-5 minutos  
**Complexidade:** Baixa  

---

**🔧 Execute `fix-and-run.bat` para correção automática!**

*Versão: 2.5.2*  
*Data: 07/01/2026*
