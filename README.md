# Blog API

Projeto de estudo feito acompanhando as aulas da Balta.io

A ideia aqui foi praticar a criação de uma API com ASP.NET Core e Entity Framework Core, entendendo melhor a estrutura do projeto, acesso a dados, validações e operações assíncronas.

## O que tem no projeto

### `HomeController`

Controller simples só para testar se a aplicação está respondendo.

### `CategoryController`

Foi o controller principal do estudo.

Nele eu trabalhei:

- listagem de categorias
- busca por id
- cadastro
- edição
- exclusão

Também usei:

- `async/await`
- `try/catch`
- `ModelState`
- retornos como `Ok`, `Created`, `BadRequest` e `NotFound`

### `Data`

A pasta `Data` tem o `BlogDataContext`, que faz a comunicação com o banco usando Entity Framework Core.

Nessa parte eu usei:

- `DbContext`
- `DbSet`
- `UseSqlServer`
- mapeamentos com Fluent API

### `Extensions`

Na pasta `Extensions` tem uma extensão para pegar os erros do `ModelState` e devolver as mensagens de validação de forma mais organizada.

### `ViewModels`

Usei `ViewModels` para separar melhor os dados recebidos pela API.

#### `CategoryViewModel`

Foi usado para validar os dados da categoria com:

- `[Required]`
- `[StringLength]`

#### `ResultViewModel`

Usei para padronizar as respostas da API, retornando dados ou erros.

## O que pratiquei nesse estudo

- criação de API com ASP.NET Core
- Entity Framework Core
- validações com Data Annotations
- operações assíncronas com `async/await`
- organização por pastas
- tratamento de erros

## Para rodar

```bash
dotnet restore
dotnet run
```

Esse projeto é só para estudo e prática dos conceitos básicos.
