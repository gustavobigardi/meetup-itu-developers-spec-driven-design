# Meetup Events API — Spec Driven Design

> Material da demo ao vivo da palestra **"Spec Driven Design"** — Meetup ITU Developers  
> **Stack**: C# · .NET 10 · VS Code · GitHub Copilot  
> **Duração da demo**: ~20 minutos

---

## O que é este repositório?

Este projeto demonstra o conceito de **Spec Driven Design** (desenvolvimento guiado pela especificação):

> **A ideia central**: antes de escrever qualquer linha de código, você define o contrato da API em um arquivo `openapi.yaml`. Esse arquivo é a "lei" do projeto — todo o código gerado deve segui-lo.

O repositório já vem com uma API de eventos de meetup parcialmente pronta. O desafio da demo é implementar ao vivo a parte de **ingressos (tickets)**, usando o GitHub Copilot como assistente — e mostrando que, com a spec aberta, o Copilot gera código que já respeita os contratos definidos.

---

## O que tem aqui

```
meetup-itu-developers-spec-driven-design/
├── EventsApi/                          # Projeto .NET da API
│   ├── .github/
│   │   └── copilot-instructions.md    # Instrui o Copilot a sempre seguir a spec
│   ├── Controllers/
│   │   └── EventsController.cs        # Endpoints de eventos — já prontos ✅
│   ├── Data/
│   │   ├── AppDbContext.cs            # Banco de dados em memória (EF Core)
│   │   └── DatabaseSeeder.cs         # 4 eventos de exemplo pré-carregados
│   ├── DTOs/                          # Objetos de request/response
│   ├── Models/                        # Entidades do banco de dados
│   ├── Services/
│   │   ├── IEventsService.cs          # Interface do serviço de eventos ✅
│   │   └── EventsService.cs           # Implementação do serviço de eventos ✅
│   ├── openapi.yaml                   # 📋 A spec — fonte da verdade do projeto
│   └── requests.http                  # Requisições prontas para testar no VS Code
└── Slides.pdf                         # Slides da palestra
```

### O que já está implementado

| Endpoint | O que faz | Status |
|---|---|---|
| `GET /events` | Lista todos os eventos | ✅ Pronto |
| `POST /events` | Cria um novo evento | ✅ Pronto |
| `GET /events/{id}` | Busca um evento pelo ID | ✅ Pronto |
| `PUT /events/{id}` | Atualiza um evento | ✅ Pronto |
| `DELETE /events/{id}` | Cancela um evento | ✅ Pronto |
| `GET /events/{id}/tickets` | Lista ingressos do evento | 🔜 Será feito na demo |
| `POST /events/{id}/tickets` | Compra um ingresso | 🔜 Será feito na demo |

### Dados de exemplo (já carregados)

| ID | Evento | Status | Vagas disponíveis | Preço |
|---|---|---|---|---|
| `evt-001` | Meetup ITU — Spec Driven Design | publicado | 37 de 100 | Gratuito |
| `evt-002` | Workshop Clean Architecture | publicado | 15 de 40 | R$ 50 |
| `evt-003` | Tech Talk: IA no Cotidiano | **rascunho** | 60 de 60 | Gratuito |
| `evt-004` | Hackathon ITU Dev 2025 | publicado | **0 de 80** | R$ 30 |

> 💡 O `evt-004` com 0 vagas é proposital — serve para demonstrar o erro `409 SOLD_OUT` na demo.  
> 💡 O `evt-003` em rascunho serve para demonstrar a atualização de status com `PUT`.

---

## Pré-requisitos

Antes de começar, instale e configure:

**1. .NET 10**
```bash
dotnet --version   # deve aparecer 10.x.x
```
Não tem? Baixe em: https://dotnet.microsoft.com/download

**2. VS Code com as extensões abaixo:**
- **GitHub Copilot** + **GitHub Copilot Chat** — o assistente de IA
- **REST Client** (`humao.rest-client`) — para executar os arquivos `.http`
- **YAML** (by Red Hat) — para editar o `openapi.yaml` com destaque de sintaxe

**3. Conta GitHub com acesso ao Copilot**  
Verifique no VS Code: `Ctrl+Shift+P` → `GitHub Copilot: Sign In`

---

## Como rodar o projeto

```bash
# 1. Entre na pasta do projeto
cd EventsApi

# 2. Inicie a API
dotnet run
```

A API estará disponível em `http://localhost:5000`.

Para testar, abra o arquivo `EventsApi/requests.http` no VS Code e clique em **"Send Request"** acima de qualquer requisição.

---

## Passo a passo da demo

> 🎯 **Objetivo da demo**: mostrar que, com a spec definida antes do código, o Copilot consegue implementar novos endpoints com muito menos esforço — e já respeitando todos os contratos documentados.

---

### Etapa 0 — Ver a API funcionando (2 min)

Com a API rodando (`dotnet run`), abra `requests.http` e execute:

- **Requisição #1** → `GET /events` — retorna os 4 eventos do banco
- **Requisição #3** → `GET /events/evt-001` — detalhe do Meetup SDD
- **Requisição #6** → `POST /events` com dados inválidos — retorna `400 VALIDATION_ERROR`

> 💬 *"A API já existe, já está rodando, já tem dados reais. O CRUD de eventos está 100% pronto. Mas olha o que a spec já define..."*

---

### Etapa 1 — Ver a spec e o gap (5 min)

Abra o arquivo `EventsApi/openapi.yaml` no VS Code. Role até a seção de tickets:

```yaml
# =============================================
# TICKETS — 🔜 Especificado, aguardando implementação
# =============================================
/events/{eventId}/tickets:
```

> 💬 *"Os endpoints de tickets já estão documentados aqui — os campos, as validações, os status codes de erro, tudo. O contrato existe. O código é que ainda não existe."*

Agora execute a **Requisição #9** do `requests.http`:
```
GET http://localhost:5000/events/evt-001/tickets
```

Resultado: `404 Not Found`. A rota não existe no código — mas já está na spec. **Isso é exatamente o ponto.**

---

### Etapa 2 — Implementar tickets com o Copilot (13 min)

> ⚠️ **Importante**: antes de começar, deixe o arquivo `openapi.yaml` aberto em uma aba do VS Code. O Copilot lê todos os arquivos abertos para montar o contexto — com a spec visível, ele vai gerar código que já segue os contratos definidos.

---

#### Passo 1 — Criar os DTOs (2 min)

DTOs (Data Transfer Objects) são os objetos que representam os dados de entrada e saída da API. Vamos criar dois: um para representar um ingresso na resposta, e outro para representar os dados de quem está comprando.

No **Copilot Chat**, cole o prompt:

```
Baseado na spec openapi.yaml aberta, crie os DTOs para os endpoints de tickets:
- DTOs/TicketDto.cs (schema Ticket)
- DTOs/PurchaseTicketRequest.cs (schema PurchaseTicketRequest, com validações)

Siga exatamente os nomes de campos e constraints da spec.
```

---

#### Passo 2 — Criar o Service (4 min)

O Service é a camada que contém a lógica de negócio — validações, regras, acesso ao banco. No Copilot Chat:

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

---

#### Passo 3 — Criar o Controller (4 min)

O Controller é a camada que recebe as requisições HTTP e chama o Service. No Copilot Chat:

```
Crie Controllers/TicketsController.cs com os dois endpoints de tickets da spec:

- GET /events/{eventId}/tickets → operationId: listEventTickets
  - 200: lista de tickets
  - 404: evento não encontrado (ErrorResponse com code NOT_FOUND)

- POST /events/{eventId}/tickets → operationId: purchaseTicket
  - 201: ingresso criado
  - 404: evento não encontrado
  - 409: sem vagas (ErrorResponse com code EVENT_SOLD_OUT)

Injete ITicketsService no construtor (primary constructor).
```

---

#### Passo 4 — Registrar o Service (1 min)

Para que o .NET saiba como criar o `TicketsService` quando o controller precisar, precisamos registrá-lo. No Copilot Chat:

```
No Program.cs, adicione o registro do ITicketsService:
builder.Services.AddScoped<ITicketsService, TicketsService>();
```

---

#### Passo 5 — Testar tudo (2 min)

Pare e reinicie a API (`Ctrl+C` e `dotnet run`), depois teste com `requests.http`:

| Requisição | Endpoint | Resultado esperado |
|---|---|---|
| #9 | `GET /events/evt-001/tickets` | `200` — lista vazia |
| #10 | `POST /events/evt-001/tickets` | `201` — ingresso criado |
| #11 | `POST /events/evt-004/tickets` | `409 EVENT_SOLD_OUT` 🎯 |
| #12 | `POST /events/nao-existe/tickets` | `404 NOT_FOUND` |

> 💬 *"O `evt-004` é o Hackathon com 0 vagas. A spec documentava esse caso, implementamos a regra, e o retorno é exatamente o que foi especificado. O contrato funcionou."*

---

## Conceitos-chave da demo

| Conceito | O que significa |
|---|---|
| **Spec Driven Design** | Escrever o contrato da API antes de qualquer código |
| **openapi.yaml** | O arquivo que define todos os endpoints, campos e regras da API |
| **operationId** | Nome único de cada endpoint na spec — vira o nome do método no código |
| **DTO** | Objeto que representa os dados que entram ou saem da API |
| **Service** | Camada que contém as regras de negócio |
| **Controller** | Camada que recebe as requisições HTTP e responde ao cliente |
| **neighboring tabs** | O Copilot lê os arquivos abertos no VS Code para montar o contexto |

---

## Se algo der errado durante a demo

```bash
# Se o código travar, a spec completa está em:
EventsApi/openapi.yaml

# Mostre o arquivo openapi.yaml e explique o que deveria ter sido gerado.
# Mantenha uma branch com a implementação completa como backup.
```
