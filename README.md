# Sistema de Gerenciamento de Tarefas

Sistema completo para gerenciamento de tarefas com .NET 8 (backend) e Angular 17 (frontend).

## 🚀 Como Executar

### Docker (Recomendado)

```bash
# 1. Clone o repositório
git clone <url-do-repositorio>
cd desafio-mv

# 2. Execute os containers
docker-compose up -d

# 3. Acesse as aplicações
# Frontend: http://localhost:8080
# Backend: http://localhost:5000
# Swagger: http://localhost:5000/swagger

# Credenciais de acesso:
# Usuário: admin
# Senha: mv
```

### Execução Manual

**Backend:**

```bash
cd tarefa-api
dotnet restore
dotnet ef database update --project Tarefas.Infra --startup-project Tarefas.API
dotnet run --project Tarefas.API
```

**Frontend:**

```bash
cd tarefa-app
npm install
ng serve
```

## 🛠️ Tecnologias

### Backend

- **.NET 8** + ASP.NET Core Web API
- **Entity Framework Core** + MySQL
- **JWT Bearer** (Autenticação)
- **AutoMapper** + **FluentValidation**
- **Swagger** (Documentação)
- **xUnit + Moq** (Testes)

### Frontend

- **Angular 17** + TypeScript
- **Angular Material** + Bootstrap 5
- **RxJS** (Programação reativa)

### Infraestrutura

- **Docker** + Docker Compose
- **MySQL** (Banco de dados)
- **Nginx** (Servidor web)

## 🏗️ Arquitetura

### Backend - Clean Architecture

```
Tarefas.API/          # Controllers + Middleware
Tarefas.Application/  # Serviços + DTOs + Validações
Tarefas.Domain/       # Entidades + Interfaces
Tarefas.Infra/        # Repositórios + DbContext
Tarefas.Ioc/          # Injeção de Dependência
Tarefas.Tests/        # Testes Unitários
```

### Frontend - Componentes

```
src/app/
├── components/       # Login + Tarefas (CRUD)
├── services/         # HTTP Services
├── guards/           # Autenticação
└── interfaces/       # Contratos TypeScript
```

## 📋 Funcionalidades

- ✅ **Autenticação JWT**
- ✅ **CRUD completo de tarefas**
- ✅ **Filtros e paginação**
- ✅ **Validação de dados**
- ✅ **Interface responsiva**
- ✅ **Documentação Swagger**

## 🔧 Decisões Técnicas

### **Separação em Camadas**

- **API REST** para backend isolado
- **SPA Angular** para frontend responsivo
- **MySQL containerizado** para persistência

### **Segurança & Validação**

- **JWT stateless** para autenticação
- **BCrypt** para hash de senhas
- **FluentValidation** no backend
- **Angular Guards** no frontend

### **Testes & Qualidade**

- **Testes unitários** com xUnit/Moq
- **InMemory Database** para testes
- **Repository Pattern** para abstração
- **Middleware** para tratamento global de erros

### **Performance**

- **Lazy Loading** no Angular
- **Connection Pooling** no EF Core
- **Docker multi-stage builds**
- **Nginx** para servir arquivos estáticos

## 🧪 Executar Testes

```bash
# Backend
cd tarefa-api && dotnet test
```

---

**Desenvolvido como desafio técnico MV**
