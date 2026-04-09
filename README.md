# Blog API

Projeto de estudo feito acompanhando as aulas da Balta.io.

A proposta deste projeto foi praticar a criacao de uma API com ASP.NET Core, Entity Framework Core e, na etapa atual, autenticacao com JWT.

## O que foi desenvolvido

### Categorias

O projeto possui um CRUD de categorias com os endpoints de:

- listagem
- busca por id
- cadastro
- edicao
- exclusao

Nessa parte foram praticados:

- `async/await`
- validacao com `ModelState`
- `try/catch`
- retorno padronizado de respostas

### Autenticacao com JWT

Foi adicionada uma estrutura inicial de autenticacao usando token JWT.

O projeto agora conta com:

- geracao de token em `AccountController`
- `TokenService` para emissao do JWT
- configuracao de autenticacao no `Program.cs`
- endpoints protegidos com `[Authorize]`
- controle de acesso por perfil usando roles como:
  - `user`
  - `author`
  - `admin`

## Estrutura principal

### `Controllers`

- `HomeController`
  Endpoint simples para teste da aplicacao.

- `CategoryController`
  Controller principal do CRUD de categorias.

- `AccountController`
  Controller criado para login e teste de rotas autenticadas com JWT.

### `Data`

A pasta `Data` contem o `BlogDataContext`, responsavel pelo acesso ao banco com Entity Framework Core.

Nessa parte foram utilizados:

- `DbContext`
- `DbSet`
- `UseSqlServer`
- mapeamentos com Fluent API

### `Services`

A pasta `Services` contem o `TokenService`, responsavel por gerar o token JWT da aplicacao.

### `Extensions`

Foi criada uma extensao para ler os erros do `ModelState` e retornar mensagens de validacao de forma mais organizada.

### `ViewModels`

Os `ViewModels` foram usados para separar melhor os dados recebidos e retornados pela API.

Destaques:

- `CategoryViewModel`
- `ResultViewModel`

## Tecnologias utilizadas

- ASP.NET Core
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- C#

## O que foi praticado

- criacao de API REST com ASP.NET Core
- organizacao por pastas
- validacoes com Data Annotations
- operacoes assincronas com `async/await`
- autenticacao com JWT
- autorizacao com roles
- tratamento de erros

## Como executar

```bash
dotnet restore
dotnet run
```

## Testando no Postman

Quem quiser testar a API pode importar o arquivo `Blog-Postman.Json` no Postman.

Nele e possivel testar endpoints como:

- `GET`
- `POST`
- `PUT`
- `DELETE`
- rotas autenticadas com token

## Observacao

Este projeto tem foco em estudo e pratica. A implementacao atual foi usada para consolidar fundamentos de API, persistencia com EF Core e autenticacao com JWT.
