# 🔍 CodeQL Security Analysis - Hotelaria

**Data:** 08/01/2026  
**Versão:** v2.6.4  
**Autor:** Nicolas Rosa

---

## 🎯 O Que é CodeQL?

**CodeQL** é uma ferramenta de análise de código que:

- ✅ **Detecta vulnerabilidades** de segurança
- ✅ **Encontra bugs** potenciais
- ✅ **Analisa padrões** de código inseguro
- ✅ **Executa automaticamente** em cada push
- ✅ **Suporta múltiplas linguagens**

---

## 📊 Linguagens Analisadas

### 1. **C# (.NET 8 / Blazor)**
```
Arquivos analisados:
- Pages/**/*.razor
- Services/**/*.cs
- Models/**/*.cs
- Shared/**/*.razor
- Program.cs
```

**Vulnerabilidades detectadas:**
- SQL Injection
- XSS (Cross-Site Scripting)
- Path Traversal
- Insecure Deserialization
- Hardcoded Credentials
- Weak Cryptography

---

### 2. **JavaScript**
```
Arquivos analisados:
- wwwroot/js/**/*.js
```

**Vulnerabilidades detectadas:**
- XSS (DOM-based)
- Prototype Pollution
- Regular Expression DoS (ReDoS)
- Unvalidated Redirects
- Insecure Random
- eval() usage

---

### 3. **GitHub Actions**
```
Arquivos analisados:
- .github/workflows/**/*.yml
```

**Vulnerabilidades detectadas:**
- Script Injection
- Secrets Exposure
- Insecure Checkout
- Dangerous Permissions
- Unverified Actions

---

## 🚀 Como Funciona

### Workflow Automático

```yaml
Trigger:
  - Push em main/develop
  - Pull Request para main
  - Agendado: Sábados às 23:32 UTC
  - Manual (workflow_dispatch)

Passos:
  1. Checkout do código
  2. Setup .NET 8
  3. Restore dependencies
  4. Initialize CodeQL
  5. Autobuild (C#) / Análise (JS/Actions)
  6. Perform Analysis
  7. Upload results
```

---

## 📝 Configuração

### Arquivo Principal
**`.github/workflows/codeql.yml`**

```yaml
name: "🔍 CodeQL Security Analysis"

on:
  push:
    branches: [ "main", "develop" ]
  pull_request:
    branches: [ "main" ]
  schedule:
    - cron: '32 23 * * 6'
  workflow_dispatch:

jobs:
  analyze:
    runs-on: ubuntu-latest
    strategy:
      matrix:
        language: [ 'csharp', 'javascript-typescript', 'actions' ]
```

### Configuração Avançada
**`.github/codeql/codeql-config.yml`**

```yaml
queries:
  - uses: security-extended
  - uses: security-and-quality

paths-ignore:
  - '**/obj/**'
  - '**/bin/**'
  - '**/node_modules/**'

paths:
  - 'Pages/**'
  - 'Services/**'
  - 'Models/**'
```

---

## 🔍 Ver Resultados

### No GitHub

1. **Security Tab**
   ```
   Repository → Security → Code scanning
   https://github.com/avilaops/hotelaria/security/code-scanning
   ```

2. **Alerts**
   ```
   Lista de vulnerabilidades encontradas
   Ordenadas por severidade:
   - 🔴 Critical
   - 🟠 High
   - 🟡 Medium
   - 🔵 Low
   - ⚪ Note
   ```

3. **Details**
   ```
   Clicar em um alerta para ver:
   - Descrição da vulnerabilidade
   - Código afetado
   - Recomendações de correção
   - CWE ID
   - CVSS Score
   ```

---

## 📊 Dashboard de Segurança

### Métricas Disponíveis

```
╔════════════════════════════════════════╗
║  CODEQL SECURITY DASHBOARD            ║
╠════════════════════════════════════════╣
║  Total Scans:            ✅ XX        ║
║  Open Alerts:            🔴 XX        ║
║  Closed Alerts:          ✅ XX        ║
║  False Positives:        ⚪ XX        ║
╠════════════════════════════════════════╣
║  By Severity:                          ║
║  - Critical:             🔴 XX        ║
║  - High:                 🟠 XX        ║
║  - Medium:               🟡 XX        ║
║  - Low:                  🔵 XX        ║
╚════════════════════════════════════════╝
```

---

## 🛠️ Executar Manualmente

### Via GitHub Actions

```
1. Ir para: https://github.com/avilaops/hotelaria/actions
2. Selecionar: "CodeQL Security Analysis"
3. Clicar: "Run workflow"
4. Escolher branch: main ou develop
5. Clicar: "Run workflow"
6. Aguardar: ~5-10 minutos
7. Ver resultados: Security tab
```

### Via CLI (Local)

```bash
# Instalar CodeQL CLI
# https://github.com/github/codeql-cli-binaries/releases

# Criar database
codeql database create hotelaria-db --language=csharp

# Executar queries
codeql database analyze hotelaria-db \
  csharp-security-extended.qls \
  --format=sarif-latest \
  --output=results.sarif

# Ver resultados
codeql database interpret-results hotelaria-db results.sarif
```

---

## 🔧 Corrigir Vulnerabilidades

### Passo a Passo

#### 1. **Identificar Alerta**
```
Security → Code scanning → Selecionar alerta
```

#### 2. **Analisar Código**
```
Ver arquivo e linha afetada
Entender o problema
Ler recomendações
```

#### 3. **Corrigir Código**
```
Editar arquivo
Implementar fix sugerido
Testar localmente
```

#### 4. **Commit e Push**
```bash
git add .
git commit -m "🔒 fix: Resolve CodeQL alert #123"
git push origin main
```

#### 5. **Verificar Correção**
```
Aguardar nova análise
Verificar se alerta foi fechado
```

#### 6. **Fechar Manualmente** (se false positive)
```
Security → Code scanning → Alerta
Clicar: "Dismiss"
Motivo: False positive / Won't fix / Used in tests
Comentário: Explicar por que
```

---

## 📝 Exemplos de Vulnerabilidades

### 1. SQL Injection (CWE-89)

**❌ Código Vulnerável:**
```csharp
var query = $"SELECT * FROM Users WHERE Username = '{username}'";
```

**✅ Código Seguro:**
```csharp
var query = "SELECT * FROM Users WHERE Username = @username";
cmd.Parameters.AddWithValue("@username", username);
```

---

### 2. XSS (CWE-79)

**❌ Código Vulnerável:**
```razor
<div>@Html.Raw(userInput)</div>
```

**✅ Código Seguro:**
```razor
<div>@userInput</div>  <!-- Blazor escapa automaticamente -->
```

---

### 3. Path Traversal (CWE-22)

**❌ Código Vulnerável:**
```csharp
var path = Path.Combine(baseDir, userInput);
File.ReadAllText(path);
```

**✅ Código Seguro:**
```csharp
var sanitized = Path.GetFileName(userInput);
var path = Path.Combine(baseDir, sanitized);
if (path.StartsWith(baseDir))
    File.ReadAllText(path);
```

---

### 4. Hardcoded Credentials (CWE-798)

**❌ Código Vulnerável:**
```csharp
var password = "admin123";
```

**✅ Código Seguro:**
```csharp
var password = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");
```

---

### 5. Weak Cryptography (CWE-327)

**❌ Código Vulnerável:**
```csharp
using (var md5 = MD5.Create())
{
    var hash = md5.ComputeHash(bytes);
}
```

**✅ Código Seguro:**
```csharp
using (var sha256 = SHA256.Create())
{
    var hash = sha256.ComputeHash(bytes);
}
```

---

## 🚨 Alertas Comuns no Hotelaria

### Potenciais Vulnerabilidades

1. **AuthService.cs**
   - Hash de senhas (SHA256 → PBKDF2)
   - Validação de input

2. **ImportacaoService.cs**
   - Path Traversal
   - File validation
   - CSV Injection

3. **Login.razor**
   - XSS em mensagens de erro
   - Timing attacks

4. **wwwroot/js/*.js**
   - DOM-based XSS
   - Unvalidated redirects

---

## 📊 Métricas e KPIs

### Metas

```
╔════════════════════════════════════════╗
║  SECURITY GOALS                       ║
╠════════════════════════════════════════╣
║  Critical Alerts:        🎯 0         ║
║  High Alerts:            🎯 < 5       ║
║  Medium Alerts:          🎯 < 10      ║
║  Time to Fix:            🎯 < 7 days  ║
║  False Positive Rate:    🎯 < 10%     ║
╚════════════════════════════════════════╝
```

### Tracking

```bash
# Ver estatísticas no GitHub
# Security → Overview → Insights
```

---

## 🔄 Integração com CI/CD

### Workflow Principal

**`.github/workflows/dotnet.yml`**

```yaml
jobs:
  security:
    name: 🔒 Security Analysis
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
      - uses: github/codeql-action/analyze@v3
```

### Status Badge

```markdown
![CodeQL](https://github.com/avilaops/hotelaria/actions/workflows/codeql.yml/badge.svg)
```

---

## 📚 Documentação Oficial

### Links Úteis

- **CodeQL Documentation**
  - https://codeql.github.com/docs/

- **Security Queries**
  - https://github.com/github/codeql/tree/main/csharp/ql/src/Security

- **Best Practices**
  - https://docs.github.com/en/code-security/code-scanning/automatically-scanning-your-code-for-vulnerabilities-and-errors/about-code-scanning

- **CWE Reference**
  - https://cwe.mitre.org/

---

## 🎓 Treinamento

### Aprender CodeQL

1. **CodeQL for Security Researchers**
   - https://securitylab.github.com/ctf/

2. **Writing Custom Queries**
   - https://codeql.github.com/docs/writing-codeql-queries/

3. **Query Language Reference**
   - https://codeql.github.com/docs/ql-language-reference/

---

## ✅ Checklist de Segurança

### Implementação

- [x] Workflow CodeQL criado
- [x] Configuração personalizada
- [x] Análise de C#
- [x] Análise de JavaScript
- [x] Análise de Actions
- [x] Queries de segurança habilitadas
- [x] Agendamento semanal
- [ ] Primeira análise executada
- [ ] Alertas revisados
- [ ] Vulnerabilidades corrigidas

### Manutenção

- [ ] Revisar alertas semanalmente
- [ ] Atualizar queries trimestralmente
- [ ] Treinar equipe em segurança
- [ ] Documentar false positives
- [ ] Monitorar novas vulnerabilidades

---

## 🎉 Resultado Esperado

Após configuração e primeira análise:

```
╔════════════════════════════════════════╗
║  CODEQL ANALYSIS COMPLETE             ║
╠════════════════════════════════════════╣
║  Languages Analyzed:     ✅ 3         ║
║  Files Scanned:          ✅ 50+       ║
║  Queries Executed:       ✅ 200+      ║
║  Vulnerabilities Found:  🔍 X         ║
╠════════════════════════════════════════╣
║  Status: Ready for Review             ║
╚════════════════════════════════════════╝
```

---

## 📞 Suporte

### Issues Comuns

**Q: CodeQL está falhando no build C#**

**A:** Verificar:
```yaml
# .github/workflows/codeql.yml
- name: Setup .NET
  uses: actions/setup-dotnet@v4
  with:
    dotnet-version: '8.0.x'
```

**Q: Muitos false positives**

**A:** Adicionar em `.github/codeql/codeql-config.yml`:
```yaml
query-filters:
  - exclude:
      id: specific-query-id
```

**Q: Análise muito lenta**

**A:** Reduzir escopo:
```yaml
paths:
  - 'Services/**'  # Apenas serviços críticos
```

---

## 🔐 Compliance

CodeQL ajuda com:

- ✅ **OWASP Top 10**
- ✅ **CWE Top 25**
- ✅ **SANS Top 25**
- ✅ **PCI DSS**
- ✅ **LGPD / GDPR**

---

**Versão:** v2.6.4  
**Data:** 08/01/2026  
**Status:** ✅ CodeQL Configurado

---

**🔒 Código seguro é código confiável!**

*Ávila Inc. - Desenvolvido com ❤️ em Portugal*
