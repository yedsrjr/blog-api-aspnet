# Blog API

Projeto baseado em estudos de APIs .NET, evoluído com foco em autenticação, arquitetura e boas práticas

O foco deste repositorio e praticar:

- criacao de API REST
- organizacao por camadas e pastas
- validacao com Data Annotations e `ModelState`
- persistencia com EF Core
- autenticacao e autorizacao com JWT + roles

## Arquitetura

O projeto foi estruturado visando baixo acoplamento e facilidade de manutenção.

O fluxo da aplicação segue:

Controller → Service → Data

- Controllers: recebem requisições e validam entrada
- Services: concentram regras de negócio
- Data: acesso ao banco via EF Core

Essa separação permite evoluir regras sem impactar outras camadas.

## Funcionalidades

### Categorias

CRUD completo de categorias com endpoints para:

- listar
- buscar por id
- cadastrar
- editar
- remover

### Autenticacao

Fluxo de autenticacao com:

- `POST /v1/accounts/` para cadastro de usuario
- `POST /v1/accounts/login` para login
- geracao de token JWT em `TokenService`
- claims montadas a partir do usuario e dos seus roles

## Estrutura de pastas

```text
Blog/
|-- Controllers/
|   |-- AccountController.cs
|   |-- CategoryController.cs
|   `-- HomeController.cs
|-- Data/
|   |-- BlogDataContext.cs
|   `-- Mappings/
|       |-- CategoryMap.cs
|       |-- PostMap.cs
|       `-- UserMap.cs
|-- Extensions/
|   |-- Auth/
|   |   `-- RoleClaimsExtension.cs
|   `-- Validation/
|       `-- ModelStateExtension.cs
|-- Migrations/
|-- Models/
|   |-- Category.cs
|   |-- Post.cs
|   |-- Role.cs
|   |-- Tag.cs
|   `-- User.cs
|-- Services/
|   `-- TokenService.cs
|-- ViewModels/
|   |-- Account/
|   |   |-- LoginViewModel.cs
|   |   `-- RegisterViewModel.cs
|   |-- Category/
|   |   `-- CategoryViewModel.cs
|   `-- Shared/
|       `-- ResultViewModel.cs
|-- Blog-Postman.json
|-- Configuration.cs
|-- Program.cs
`-- README.md
```

## Responsabilidades por pasta

### `Controllers`

Recebem as requisicoes HTTP, validam entrada, chamam a camada de dados/servicos e devolvem a resposta da API.

### `Data`

Centraliza o acesso ao banco de dados:

- `BlogDataContext` com os `DbSet`s
- mapeamentos Fluent API em `Mappings/`
- migrations geradas pelo EF Core

### `Extensions`

Agrupa extensoes reutilizaveis da aplicacao:

- `Auth/RoleClaimsExtension`: transforma roles do usuario em claims do token
- `Validation/ModelStateExtension`: converte erros de validacao em lista padronizada

### `Models`

Entidades principais do dominio persistidas no banco.

### `Services`

Servicos de suporte da aplicacao, como a emissao de JWT.

### `ViewModels`

Modelos de entrada e saida organizados por contexto:

- `Account/`: login e cadastro
- `Category/`: payloads de categoria
- `Shared/`: respostas padronizadas

## Autenticacao e roles

O token JWT e configurado no `Program.cs` e gerado pelo `TokenService`.

No login:

1. o usuario informa email e senha
2. o sistema valida o payload com `LoginViewModel`
3. o usuario e carregado com seus roles
4. o token e emitido com claims de identificacao e permissao

Isso prepara o projeto para uso com `[Authorize]` e regras baseadas em roles.

## Como executar

```bash
dotnet restore
dotnet run
```

## Testes (em evolução)

O projeto está sendo evoluído para incluir testes unitários focados na camada de serviço, garantindo a consistência das regras de negócio.

## Tecnologias

- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- C#

## Observacao

Projeto em evolução contínua com foco em boas práticas de desenvolvimento backend
