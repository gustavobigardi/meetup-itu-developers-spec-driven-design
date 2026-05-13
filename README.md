# Demo — Meetup Events API

> **Contexto**: Demo ao vivo da palestra "Spec Driven Design" — Meetup ITU Developers  
> **Stack**: C# .NET 10 · VS Code · GitHub Copilot (GPT-5 mini)  
> **Duração da demo**: ~20 minutos

---

## Estrutura do Projeto

```
demo/
└── EventsApi/                  # Projeto .NET já funcional
    ├── .github/
    │   └── copilot-instructions.md   # Copilot sempre referencia a spec
    ├── Controllers/
    │   └── EventsController.cs       # CRUD de eventos — já implementado ✅
    ├── Data/
    │   ├── AppDbContext.cs
    │   └── DatabaseSeeder.cs         # 4 eventos realistas no seed
    ├── DTOs/                         # Request/Response objects
    ├── Models/                       # Entidades EF Core
    ├── Services/
    │   ├── IEventsService.cs         # ✅ implementado
    │   └── EventsService.cs          # ✅ implementado
    ├── openapi.yaml                  # Spec: Events ✅  Tickets 🔜
    └── requests.http                 # Requisições para VS Code REST Client
```

---

## Pré-requisitos

```bash
# Verificar .NET
dotnet --version   # 10.x ou superior

# VS Code — Extensões necessárias:
# - GitHub Copilot + GitHub Copilot Chat
# - REST Client (humao.rest-client)
# - YAML (by Red Hat)
```

---

## Roteiro da Demo (20 min)

### Fase 0 — Mostrar a API rodando (2 min)

```bash
cd demo/EventsApi
dotnet run
```

Abra `requests.http` no VS Code e execute:
- **Request #1** — `GET /events` → retorna 4 eventos do seed
- **Request #3** — `GET /events/evt-001` → detalhe do Meetup SDD
- **Request #6** — `POST /events` com dados inválidos → `400 VALIDATION_ERROR`

> 💬 **Fala**: "A API já existe, já está rodando, já tem eventos reais. O CRUD de eventos está 100% implementado. Mas olha o que está na spec..."

---

### Fase 1 — Mostrar a spec e o gap (5 min)

Abra `openapi.yaml`. Aponte para o cabeçalho da spec:

```yaml
# ============================================================
# STATUS DOS ENDPOINTS
# ============================================================
# Events  (GET/POST /events, GET/PUT/DELETE /events/{id}) ✅
# Tickets (GET/POST /events/{eventId}/tickets)            🔜
# ============================================================
```

> 💬 **Fala**: "Olha aqui — tickets já estão **na spec**. O contrato foi definido primeiro. Os schemas, os status codes, os exemplos, as validações — tudo documentado. Mas o código ainda não existe."

Clique no Request #9 do `requests.http`:
```http
GET http://localhost:5000/events/evt-001/tickets
```

Recebe `404` (rota não existe). **É exatamente o que esperamos** — a spec foi à frente do código.

---

### Fase 2 — Implementar tickets com Copilot (13 min)

**Deixe `openapi.yaml` aberto em uma aba.** O Copilot vai usá-lo automaticamente via *neighboring tabs*.

#### Passo 1 — DTOs (2 min)

No Copilot Chat:

```
Baseado na spec openapi.yaml aberta, crie os DTOs para os endpoints de tickets:
- DTOs/TicketDto.cs (schema Ticket)
- DTOs/PurchaseTicketRequest.cs (schema PurchaseTicketRequest, com validações)

Siga exatamente os nomes de campos e constraints da spec.
```

#### Passo 2 — Service (4 min)

```
Crie a interface ITicketsService e a implementação TicketsService em Services/.

Operações necessárias (baseadas nos endpoints da spec):
- ListTickets(eventId): retorna lista de TicketDto ou null se evento não existe
- PurchaseTicket(eventId, request): cria ingresso, decrementa availableSpots
  - Retorna null se evento não existe
  - Retorna string "SOLD_OUT" se availableSpots == 0
  - Retorna o TicketDto criado se sucesso

Use o AppDbContext via injeção de dependência, como EventsService faz.
```

#### Passo 3 — Controller (4 min)

```
Crie Controllers/TicketsController.cs com os dois endpoints de tickets da spec:

- GET /events/{eventId}/tickets → operationId: listTickets
  - 200: lista de tickets
  - 404: evento não encontrado (ErrorResponse com code NOT_FOUND)

- POST /events/{eventId}/tickets → operationId: purchaseTicket
  - 201: ingresso criado
  - 404: evento não encontrado
  - 409: sem vagas (ErrorResponse com code SOLD_OUT)

Injete ITicketsService no construtor (primary constructor).
```

#### Passo 4 — Registrar no Program.cs (1 min)

```
No Program.cs, adicione o registro do ITicketsService:
builder.Services.AddScoped<ITicketsService, TicketsService>();
```

#### Passo 5 — Testar (2 min)

Reinicie a API e use `requests.http`:

- **Request #9** — `GET /events/evt-001/tickets` → `200` (lista vazia)
- **Request #10** — `POST /events/evt-001/tickets` → `201` (ingresso criado)
- **Request #11** — `POST /events/evt-004/tickets` → `409 SOLD_OUT` 🎯

> 💬 **Fala**: "Veja — o evt-004 é o Hackathon com 0 vagas. A spec documentava esse caso, implementamos o tratamento, e o comportamento é exatamente o que foi especificado."

---

## Dados do Seed

| ID | Evento | Status | Vagas | Preço |
|---|---|---|---|---|
| `evt-001` | Meetup ITU — Spec Driven Design | published | 37/100 | Gratuito |
| `evt-002` | Workshop Clean Architecture | published | 15/40 | R$ 50 |
| `evt-003` | Tech Talk: IA no Cotidiano | **draft** | 60/60 | Gratuito |
| `evt-004` | Hackathon ITU Dev 2025 | published | **0/80** | R$ 30 |

> `evt-004` com 0 vagas = cenário perfeito para demonstrar o `409 Conflict`  
> `evt-003` com status draft = cenário para o `PUT /events/evt-003` publicar o evento

---

## Fallback (se algo der errado)

Se a implementação ao vivo travar:

```bash
# O arquivo openapi.yaml de referência completa está em:
demo/openapi.yaml

# Você pode mostrar o código pronto "abrindo" arquivos que já preparou.
# Mantenha uma branch/commit com a implementação completa como backup.
```


Antes de iniciar, confirme que está tudo instalado:

```bash
# Verificar .NET
dotnet --version  # deve retornar 8.x ou superior

# Verificar Node.js (para o mock server opcional)
node --version

# Copilot: verificar login no VS Code
# Ctrl+Shift+P → "GitHub Copilot: Sign In"
```

**Extensões VS Code necessárias:**
- GitHub Copilot
- GitHub Copilot Chat
- YAML (by Red Hat)
- OpenAPI (Swagger) Editor

---

## Fase 1 — Criando a Spec OpenAPI (8 min)

### Passo 1: Criar a estrutura do projeto (1 min)

```bash
mkdir meetup-events-api && cd meetup-events-api
mkdir .github
touch openapi.yaml
touch .github/copilot-instructions.md
code .
```

### Passo 2: Criar o copilot-instructions.md (2 min)

Abra `.github/copilot-instructions.md` e comece a digitar — o Copilot vai sugerir.

Ou cole diretamente o conteúdo do arquivo `.github/copilot-instructions.md` desta pasta de demo.

> 💡 **Dica para a apresentação**: Explique que este arquivo é lido automaticamente pelo Copilot em todos os chats do repositório. É como dar instruções permanentes para a IA.

### Passo 3: Criar a spec OpenAPI (5 min)

Abra `openapi.yaml` e comece digitando o cabeçalho:

```yaml
openapi: 3.0.3
info:
  title: Meetup Events API
  description: API para gerenciamento de eventos de meetup e ingressos
  version: 1.0.0
```

Em seguida, abra o **Copilot Chat** e use este prompt:

```
Preciso completar esta spec OpenAPI para uma API de gerenciamento de eventos de meetup.
Use o arquivo openapi.yaml que está aberto.

Crie os seguintes paths:
- GET /events (com parâmetros de query: status, page, pageSize)
- POST /events
- GET /events/{eventId}
- PUT /events/{eventId}
- DELETE /events/{eventId}
- GET /events/{eventId}/tickets
- POST /events/{eventId}/tickets

Crie os seguintes schemas em components/schemas:
- Event (campos: id, name, description, date, location, capacity, availableSpots, ticketPrice, status, createdAt)
- EventStatus (enum: draft, published, cancelled)
- CreateEventRequest (campos obrigatórios: name, date, location, capacity, ticketPrice)
- UpdateEventRequest (todos os campos opcionais)
- Ticket (campos: id, eventId, buyerName, buyerEmail, purchaseDate, status)
- PurchaseTicketRequest (campos: buyerName, buyerEmail)
- Error (campos: code, message, details)

Crie responses reutilizáveis: BadRequest (400), NotFound (404), UnprocessableEntity (422)

Adicione exemplos realistas de meetup em todos os schemas.
Adicione descriptions em todas as propriedades.
Use $ref para referenciar schemas nas respostas.
```

> 💡 **Se algo der errado**: Use o arquivo `openapi-start.yaml` como ponto de partida e o `openapi.yaml` como referência do resultado final.

---

## Fase 2 — Gerando Código C# .NET (12 min)

### Passo 1: Criar o projeto .NET (2 min)

```bash
dotnet new webapi -n EventsApi --no-openapi
cd EventsApi

# Remover arquivos desnecessários
rm Controllers/WeatherForecastController.cs
rm WeatherForecast.cs
```

> 💡 **Por que `--no-openapi`?** Porque não queremos gerar a spec a partir do código (code-first). Nossa spec YAML **já existe** e é o ponto de partida (spec-first).

### Passo 2: Abrir a spec ao lado do código (1 min)

No VS Code:
1. Arraste `openapi.yaml` para uma aba do lado
2. O Copilot vai pegar o contexto automaticamente via **neighboring tabs**

> 💡 **Explique para o público**: O Copilot lê todos os arquivos abertos no IDE para montar o contexto. Com a spec aberta, ele tem acesso a todos os tipos, constraints e nomes — sem precisar de nenhuma configuração extra.

### Passo 3: Gerar os Models e DTOs (3 min)

Abra o Copilot Chat e use este prompt:

```
Baseado no arquivo openapi.yaml que está aberto como aba no VS Code,
crie os seguintes arquivos C# na pasta Models/:

1. Event.cs — baseado no schema Event da spec
2. Ticket.cs — baseado no schema Ticket da spec
3. CreateEventRequest.cs — baseado no schema CreateEventRequest
4. UpdateEventRequest.cs — baseado no schema UpdateEventRequest
5. PurchaseTicketRequest.cs — baseado no schema PurchaseTicketRequest
6. ErrorResponse.cs — baseado no schema Error (com lista de ErrorDetail)

Requisitos:
- Use DataAnnotations para validação (Required, MinLength, MaxLength, Range, EmailAddress, RegularExpression)
- As constraints devem corresponder exatamente às definidas na spec (minLength, maxLength, minimum, maximum, format)
- Use tipos C# apropriados: string, int, decimal, DateTime
- Para o campo status do Event, crie um enum EventStatus com os valores da spec (Draft, Published, Cancelled)
- Para o campo status do Ticket, crie um enum TicketStatus (Active, Cancelled)
- Adicione comentários XML com as descriptions da spec
```

### Passo 4: Gerar o Controller de Eventos (4 min)

```
Baseado no arquivo openapi.yaml aberto, crie um arquivo Controllers/EventsController.cs
que implemente todos os endpoints dos paths /events e /events/{eventId}.

Requisitos:
- Use os operationIds como nomes dos métodos: listEvents, createEvent, getEventById, updateEvent, cancelEvent
- Retorne exatamente os status codes definidos na spec para cada endpoint
- Use os Models criados nos arquivos da pasta Models/
- Para o endpoint POST retorne 201 Created com o Location header
- Para o DELETE retorne 204 No Content
- Para erros 404, retorne o ErrorResponse com code "NOT_FOUND"
- Valide o ModelState em todos os endpoints que recebem body e retorne 400 se inválido
- Adicione comentários XML [summary] com o texto do campo summary de cada operação na spec
- Por enquanto, use dados em memória (uma lista estática) como repositório
```

### Passo 5: Registrar no Program.cs (2 min)

```
Atualize o Program.cs para:
1. Registrar os controllers com AddControllers()
2. Configurar JSON para usar camelCase (JsonNamingPolicy.CamelCase)
3. Mapear os controller routes com MapControllers()
4. Remover qualquer referência ao WeatherForecast
```

### Passo 6: Testar (1 min)

```bash
dotnet run
# Acesse http://localhost:5000/events no browser ou Postman
```

---

## Referências

| Arquivo | Propósito |
|---|---|
| `openapi.yaml` | Spec completa — estado final de referência |
| `openapi-start.yaml` | Spec parcial — ponto de partida para a demo ao vivo |
| `.github/copilot-instructions.md` | Instruções do Copilot |

## Mock Server (bônus, se houver tempo)

```bash
# Rodar um mock server diretamente da spec — sem escrever código!
npx @stoplight/prism-cli mock openapi.yaml

# Testar o mock
curl http://localhost:4010/events
curl -X POST http://localhost:4010/events \
  -H "Content-Type: application/json" \
  -d '{"name": "Meetup Teste", "date": "2025-12-01T19:00:00-03:00", "location": "Ituiutaba", "capacity": 50, "ticketPrice": 0}'
```

> 💡 O Prism serve respostas com os dados dos `examples` da spec — sem nenhum backend implementado!
