# Blog API

Projeto baseado em estudos de APIs .NET, evoluido com foco em autenticacao, arquitetura e boas praticas.

O foco deste repositorio e praticar:

- criacao de API REST
- organizacao por camadas e pastas
- validacao com Data Annotations e `ModelState`
- persistencia com EF Core
- autenticacao e autorizacao com JWT + roles

## Arquitetura

O projeto foi estruturado visando baixo acoplamento e facilidade de manutencao.

Fluxo principal:

`Controller -> Service -> Data`

- `Controllers`: recebem requisicoes e validam entrada
- `Services`: concentram comportamentos de suporte, como geracao de token
- `Data`: acesso ao banco via EF Core

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
|   |-- ModelStateExtension.cs
|   `-- RoleClaimsExtension.cs
|-- Migrations/
|-- Models/
|   |-- Category.cs
|   |-- Post.cs
|   |-- Role.cs
|   |-- Tag.cs
|   `-- User.cs
|-- Properties/
|   `-- launchSettings.json
|-- Services/
|   `-- TokenService.cs
|-- ViewModels/
|   |-- CategoryViewModel.cs
|   |-- LoginViewModel.cs
|   |-- RegisterViewModel.cs
|   `-- ResultViewModel.cs
|-- Blog-Postman.json
|-- Blog.csproj
|-- Blog.sln
|-- Configuration.cs
|-- Program.cs
`-- README.md
```

## Responsabilidades por pasta

### `Controllers`

Recebem as requisicoes HTTP, validam a entrada e devolvem as respostas da API.

### `Data`

Centraliza o acesso ao banco:

- `BlogDataContext` com os `DbSet`s
- mapeamentos Fluent API em `Mappings/`
- migrations geradas pelo EF Core

### `Extensions`

Agrupa extensoes reutilizaveis da aplicacao:

- `ModelStateExtension`: transforma erros de validacao em uma lista padronizada
- `RoleClaimsExtension`: converte roles do usuario em claims do token

### `Models`

Entidades principais do dominio persistidas no banco.

### `Services`

Servicos auxiliares da aplicacao, como a emissao de JWT.

### `ViewModels`

Modelos usados para entrada e saida de dados nas rotas da API.

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

## Testes de API

Para testar as rotas manualmente, importe o arquivo `Blog-Postman.json` no Postman.

## Tecnologias

- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- C#

## Observacao

Projeto em evolucao continua com foco em boas praticas de desenvolvimento backend.
