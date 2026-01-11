# ⚡ INICIAR SISTEMA - Guia Rápido

---

## 🎯 O MAIS SIMPLES

### 1. Abra o PowerShell

**Como:** Pressione `Win + X` → Escolha "Windows PowerShell"

### 2. Navegue até o projeto

```powershell
cd D:\Hotelaria
```

### 3. Execute UMA destas opções:

#### Opção A: Script automatizado (RECOMENDADO)
```powershell
.\iniciar.ps1
```

#### Opção B: Comando direto
```powershell
dotnet run
```

#### Opção C: Com hot reload
```powershell
dotnet watch run
```

### 4. Aguarde aparecer:

```
Now listening on: http://localhost:5000
```

### 5. Abra o navegador:

```
http://localhost:5000
```

---

## 🔑 Login

```
Usuário: admin
Senha: admin123
```

---

## 🛑 Parar

Pressione `Ctrl + C` no terminal

---

## ⚠️ Problemas?

### Porta em uso:
```powershell
dotnet run --urls "http://localhost:8080"
# Depois abra: http://localhost:8080
```

### Erro de compilação:
```powershell
dotnet clean
dotnet build
dotnet run
```

---

## 📖 Mais Detalhes

Ver: `docs/COMO-INICIAR-LOCAL.md`

---

**Pronto!** Em 1 minuto seu sistema está rodando localmente. 🚀
