# Copilot Instructions — Meetup Events API

## Contexto do Projeto

Esta API segue o padrão **Spec Driven Design**. A especificação OpenAPI está em `./openapi.yaml` e é a **fonte única da verdade** do projeto.

**A spec define o contrato. O código deve segui-la — nunca o contrário.**

## Regras para Geração de Código

### Endpoints e Controllers
- Todo endpoint HTTP deve corresponder a um `path` + `operationId` definido na spec
- Use o `operationId` como nome do método/action (ex: `listEvents`, `createEvent`)
- Retorne exatamente os status codes documentados na spec (201, 204, 400, 404, 409, 422...)
- Nunca retorne um campo no response que não esteja no schema da spec

### Validação de Request
- Use DataAnnotations ou FluentValidation para implementar as constraints da spec:
  - `minLength` / `maxLength` → `[MinLength]` / `[MaxLength]`
  - `minimum` / `maximum` → `[Range]`
  - `format: email` → `[EmailAddress]`
  - `required` → `[Required]`
- Retorne `400 Bad Request` com o schema `Error` quando a validação falhar

### Models e DTOs
- Os nomes dos modelos C# devem corresponder aos nomes dos schemas na spec:
  - `Event`, `Ticket`, `CreateEventRequest`, `UpdateEventRequest`, `PurchaseTicketRequest`
- Use tipos C# correspondentes aos tipos JSON Schema:
  - `string` (date-time) → `DateTime`
  - `number` (float) → `decimal`
  - `integer` → `int`
  - `string` (enum) → `enum` C#

### Respostas de Erro
- Sempre retorne erros no formato do schema `Error` definido na spec:
  ```json
  {
    "code": "VALIDATION_ERROR",
    "message": "Descrição do erro",
    "details": [{ "field": "name", "message": "O campo é obrigatório" }]
  }
  ```

## Stack Tecnológico
- **Framework**: ASP.NET Core 10 (Web API)
- **Linguagem**: C# 13
- **Padrão**: RESTful HTTP conforme OpenAPI 3.0
- **Serialização**: JSON com camelCase
- **Validação**: DataAnnotations + ModelState

## O que NÃO fazer
- ❌ Não adicione endpoints que não estão na spec sem atualizar a spec primeiro
- ❌ Não mude nomes de campos nos responses sem atualizar a spec
- ❌ Não use status codes diferentes dos documentados
- ❌ Não gere documentação OpenAPI via Swashbuckle (a spec YAML é a doc)
