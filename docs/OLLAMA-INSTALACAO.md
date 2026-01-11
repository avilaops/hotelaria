# 🤖 Guia de Instalação do Ollama

## O que é Ollama?
Ollama é uma ferramenta que permite executar modelos de IA (como Llama, Mistral, etc.) localmente no seu computador, sem precisar de internet ou serviços na nuvem.

## 📥 Instalação

### Windows (Recomendado)
1. Baixe o instalador: https://ollama.com/download/windows
2. Execute o instalador `OllamaSetup.exe`
3. Siga as instruções do instalador
4. Ollama será instalado como serviço do Windows

### Verificar Instalação
Abra o PowerShell e execute:
```powershell
ollama --version
```

## 🚀 Baixar e Executar Modelos

### Modelo Recomendado: Llama 3.2 (3B)
Este é um modelo leve e rápido, ideal para começar:

```powershell
ollama pull llama3.2
```

### Testar o Modelo
```powershell
ollama run llama3.2 "Olá, como você pode me ajudar?"
```

### Outros Modelos Disponíveis

#### Modelos Leves (até 8GB RAM)
- `llama3.2` (3B) - Rápido e eficiente
- `phi3` (3.8B) - Ótimo para raciocínio
- `gemma2:2b` (2B) - Super leve

#### Modelos Médios (12-16GB RAM)
- `mistral` (7B) - Equilibrado
- `llama3.1:8b` (8B) - Versão maior do Llama

#### Modelos Avançados (32GB+ RAM)
- `llama3.1:70b` (70B) - Alta qualidade
- `mixtral` (47B) - Modelo expert mixture

### Baixar Modelo
```powershell
# Exemplo: baixar Mistral
ollama pull mistral
```

## 🔧 Configuração no Sistema Hotelaria

### 1. Verificar se o Ollama está rodando
O Ollama roda automaticamente em `http://localhost:11434`

Teste no navegador:
```
http://localhost:11434/
```

Deve retornar: `Ollama is running`

### 2. Configurar variáveis de ambiente (opcional)

Crie ou edite o arquivo `.env` na raiz do projeto:

```env
# Ollama Configuration
OLLAMA_BASE_URL=http://localhost:11434
OLLAMA_MODEL=llama3.2
```

### 3. Executar a aplicação
```powershell
dotnet run
```

### 4. Acessar o Assistente IA
Navegue até: `http://localhost:5000/assistente-ia`

## 📊 Uso no Sistema

### Funcionalidades Disponíveis

#### 1. Chat Interativo 💬
- Faça perguntas sobre gestão hoteleira
- Peça conselhos sobre ocupação
- Tire dúvidas sobre o sistema

#### 2. Análise de Ocupação 📊
- Análise automática do mês atual
- Identificação de tendências
- Recomendações de melhorias

#### 3. Sugestão de Preços 💰
- Preços otimizados por quarto
- Considera sazonalidade e dia da semana
- Baseado em práticas de revenue management

#### 4. Relatório Financeiro 📈
- Análise de receita e despesas
- Avaliação de margem de lucro
- Sugestões de otimização

#### 5. Otimização de Descrições ✍️
- Descrições atrativas para quartos
- SEO-friendly
- Profissional e persuasiva

## 🛠️ Troubleshooting

### Ollama não está respondendo
1. Verifique se o serviço está rodando:
   ```powershell
   Get-Service | Where-Object {$_.DisplayName -like "*Ollama*"}
   ```

2. Reinicie o serviço:
   ```powershell
   Restart-Service -Name "Ollama"
   ```

3. Ou reinicie manualmente:
   - Abra o menu Iniciar
   - Procure por "Ollama"
   - Clique com botão direito → "Executar como administrador"

### Modelo demora muito para responder
- Use um modelo mais leve (como `llama3.2`)
- Verifique se seu computador tem RAM suficiente
- Feche outros aplicativos pesados

### Erro "Connection refused"
- Verifique se Ollama está instalado
- Confirme que está rodando na porta 11434
- Teste: `curl http://localhost:11434/`

### Modelo retorna respostas estranhas
- O modelo pode não estar em português
- Tente adicionar ao prompt: "Responda sempre em português de Portugal"
- Use um modelo multilíngue como `mistral`

## 🎯 Melhores Práticas

### Escolha do Modelo
- **Desenvolvimento/Testes**: `llama3.2` (rápido)
- **Produção**: `mistral` ou `llama3.1:8b` (melhor qualidade)
- **Hardware limitado**: `phi3` ou `gemma2:2b`

### Performance
- Primeira execução é sempre mais lenta (carrega modelo)
- Mantenha Ollama rodando em background
- Use SSD para armazenar modelos (mais rápido)

### Prompts Eficientes
- Seja específico no que pede
- Forneça contexto relevante
- Use português claro e direto

## 📚 Recursos Adicionais

- **Site Oficial**: https://ollama.com
- **Modelos Disponíveis**: https://ollama.com/library
- **Documentação API**: https://github.com/ollama/ollama/blob/main/docs/api.md
- **Comunidade**: https://discord.gg/ollama

## 🔐 Privacidade e Segurança

### Vantagens do Ollama
✅ **100% Local** - Nenhum dado é enviado para internet
✅ **Privado** - Dados sensíveis ficam no seu computador
✅ **Sem custos** - Não paga por tokens ou API calls
✅ **Offline** - Funciona sem internet
✅ **Controle total** - Você escolhe o modelo e configurações

### Dados que ficam locais
- Perguntas do chat
- Dados de reservas analisados
- Informações financeiras
- Descrições geradas

## 💡 Dicas Avançadas

### Usar modelo diferente para cada tarefa
Edite `Services/OllamaService.cs` e adicione:

```csharp
var modeloFinanceiro = "mistral";    // Melhor para análises
var modeloChat = "llama3.2";         // Rápido para chat
var modeloTexto = "phi3";            // Ótimo para textos
```

### Ajustar timeout
No `Program.cs`, altere:
```csharp
client.Timeout = TimeSpan.FromMinutes(10); // Aumentar para modelos lentos
```

### Executar em GPU
Se tiver NVIDIA GPU, Ollama usa automaticamente.
Verifique no Task Manager → Performance → GPU

## ✅ Checklist de Instalação

- [ ] Ollama instalado (`ollama --version` funciona)
- [ ] Modelo baixado (`ollama list` mostra modelos)
- [ ] Ollama rodando (`http://localhost:11434` responde)
- [ ] `.env` configurado (opcional)
- [ ] Aplicação compila (`dotnet build`)
- [ ] Página carrega (`/assistente-ia` abre)
- [ ] Chat funciona (recebe respostas)

## 🎓 Próximos Passos

1. Instale o Ollama
2. Baixe o modelo `llama3.2`
3. Execute a aplicação
4. Acesse `/assistente-ia`
5. Teste as ações rápidas
6. Experimente o chat

**Pronto para começar!** 🚀
