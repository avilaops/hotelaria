# Guia de Instalação Detalhado

Este guia fornece instruções passo a passo para instalar e executar o sistema Hotelaria.

## Índice
1. [Requisitos](#requisitos)
2. [Instalação no Windows](#instalação-no-windows)
3. [Instalação no Linux](#instalação-no-linux)
4. [Instalação no macOS](#instalação-no-macos)
5. [Usando Docker](#usando-docker)
6. [Configuração](#configuração)
7. [Solução de Problemas](#solução-de-problemas)

---

## Requisitos

### Obrigatórios
- .NET 8.0 SDK ou superior
- Navegador moderno (Chrome, Firefox, Edge, Safari)
- 2 GB de RAM disponível
- 500 MB de espaço em disco

### Recomendados
- Visual Studio 2022 ou VS Code
- Git para controle de versão

---

## Instalação no Windows

### 1. Instalar o .NET SDK

1. Acesse [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Baixe o instalador do .NET 8.0 SDK para Windows
3. Execute o instalador e siga as instruções
4. Verifique a instalação abrindo o PowerShell e executando:
   ```powershell
   dotnet --version
   ```

### 2. Clonar o Repositório

```powershell
# Usando HTTPS
git clone https://github.com/avilaops/hotelaria.git

# Ou usando SSH
git clone git@github.com:avilaops/hotelaria.git

# Entrar na pasta do projeto
cd hotelaria
```

### 3. Executar o Script de Inicialização

```powershell
.\start.bat
```

Ou manualmente:

```powershell
dotnet restore
dotnet build
dotnet run
```

### 4. Acessar a Aplicação

Abra seu navegador e acesse:
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

---

## Instalação no Linux

### 1. Instalar o .NET SDK

#### Ubuntu/Debian
```bash
# Adicionar repositório Microsoft
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Instalar .NET SDK
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0

# Verificar instalação
dotnet --version
```

#### Fedora
```bash
sudo dnf install dotnet-sdk-8.0
```

### 2. Clonar o Repositório

```bash
git clone https://github.com/avilaops/hotelaria.git
cd hotelaria
```

### 3. Executar o Script de Inicialização

```bash
# Dar permissão de execução
chmod +x start.sh

# Executar
./start.sh
```

Ou manualmente:

```bash
dotnet restore
dotnet build
dotnet run
```

---

## Instalação no macOS

### 1. Instalar o .NET SDK

#### Usando Homebrew (Recomendado)
```bash
brew install dotnet@8
```

#### Download Manual
1. Acesse [dotnet.microsoft.com/download](https://dotnet.microsoft.com/download/dotnet/8.0)
2. Baixe o instalador para macOS
3. Execute o instalador

Verifique a instalação:
```bash
dotnet --version
```

### 2. Clonar e Executar

```bash
git clone https://github.com/avilaops/hotelaria.git
cd hotelaria

# Dar permissão de execução
chmod +x start.sh

# Executar
./start.sh
```

---

## Usando Docker

### Pré-requisitos
- Docker instalado
- Docker Compose (opcional)

### Opção 1: Docker

```bash
# Construir a imagem
docker build -t hotelaria:latest .

# Executar o container
docker run -d -p 8080:80 --name hotelaria-app hotelaria:latest

# Acessar em http://localhost:8080
```

### Opção 2: Docker Compose

```bash
# Executar
docker-compose up -d

# Acessar em http://localhost:8080
```

### Comandos Úteis Docker

```bash
# Ver logs
docker logs hotelaria-app

# Parar container
docker stop hotelaria-app

# Remover container
docker rm hotelaria-app

# Parar e remover com Docker Compose
docker-compose down
```

---

## Configuração

### Portas

Por padrão, a aplicação usa as portas:
- **5000**: HTTP
- **5001**: HTTPS
- **8080**: Docker

Para alterar as portas, edite o arquivo `Properties/launchSettings.json`:

```json
{
  "profiles": {
    "http": {
      "applicationUrl": "http://localhost:8080"
    },
    "https": {
      "applicationUrl": "https://localhost:8443;http://localhost:8080"
    }
  }
}
```

### Variáveis de Ambiente

```bash
# Definir ambiente
export ASPNETCORE_ENVIRONMENT=Development

# Definir URLs
export ASPNETCORE_URLS="http://+:5000;https://+:5001"
```

---

## Solução de Problemas

### Erro: "dotnet: command not found"

**Solução**: O .NET SDK não está instalado ou não está no PATH.
- Reinstale o .NET SDK
- Reinicie o terminal
- Verifique a variável PATH

### Erro: "Unable to bind to https://localhost:5001"

**Solução**: A porta já está em uso.
- Verifique processos usando a porta: `netstat -ano | findstr :5001` (Windows) ou `lsof -i :5001` (Linux/macOS)
- Mate o processo ou use outra porta

### Erro de Certificado SSL

**Solução Linux/macOS**:
```bash
dotnet dev-certs https --trust
```

**Solução Windows**:
```powershell
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Erro: "The build failed"

**Solução**:
1. Limpe o projeto:
   ```bash
   dotnet clean
   ```
2. Restaure as dependências:
   ```bash
   dotnet restore
   ```
3. Compile novamente:
   ```bash
   dotnet build
   ```

### Performance Lenta

**Soluções**:
- Verifique se está executando em modo Release para produção: `dotnet run --configuration Release`
- Aumente recursos de memória disponíveis
- Desabilite hot reload em desenvolvimento: `dotnet run --no-hot-reload`

### Problemas com CSS não Carregando

**Solução**:
1. Limpe o cache do navegador (Ctrl+Shift+Delete)
2. Verifique se o arquivo `wwwroot/css/site.css` existe
3. Reconstrua o projeto: `dotnet build`

---

## Próximos Passos

Após a instalação bem-sucedida:

1. ✅ Explore o Dashboard
2. ✅ Crie algumas reservas de teste
3. ✅ Configure seus quartos
4. ✅ Adicione hóspedes
5. ✅ Explore os relatórios financeiros

---

## Suporte

Se você encontrar problemas não listados aqui:

1. Verifique as [Issues no GitHub](https://github.com/avilaops/hotelaria/issues)
2. Crie uma nova issue se necessário
3. Consulte a [documentação oficial do .NET](https://docs.microsoft.com/dotnet/)

---

**Desenvolvido com ❤️ usando Blazor**
