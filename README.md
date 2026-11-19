# CareerSearcher

CareerSearcher é uma aplicação .NET que coleta vagas de emprego e diferentes sites, salva os resultados em SQL Server e envia novas oportunidades para um webhook do Discord.

A aplicação possui uma API REST versionada, um worker em segundo plano e um scraper baseado no Microsoft Playwright. Cada pesquisa é repetida automaticamente para um conjunto de termos configurados no worker.

## Funcionalidades

- Busca vagas em diferentes sites usando Playwright e Chromium.
- Pesquisa automaticamente por:
  - `Desenvolvedor`
  - `Desenvolvedor .NET`
  - `Desenvolvedor Backend`
- Salva somente vagas novas no SQL Server.
- Evita duplicatas usando `Name + Company` como identificador único.
- Envia novas vagas como embeds para um webhook do Discord.
- Expõe vagas armazenadas em uma API REST paginada.
- Disponibiliza uma interface web para pesquisar e visualizar vagas.
- Disponibiliza a documentação Swagger/OpenAPI.
- Executa migrações/criação do banco na inicialização da API.

## Tecnologias

- .NET 10
- ASP.NET Core
- MediatR
- Entity Framework Core 10
- SQL Server 2025
- Microsoft Playwright
- Discord Webhooks
- Docker e Docker Compose
- xUnit/Moq
- GitHub Actions
- React, TypeScript e Vite

## Arquitetura

O projeto segue uma separação em camadas:

```text
CareerSearcher/
├── src/
│   ├── CareerSearcher.API
│   ├── CareerSearcher.Application
│   ├── CareerSearcher.Domain
│   ├── CareerSearcher.Infrastructure
│   └── CareerSearcher.Tests
└── frontend/
    └── aplicação React com Vite
```

- **Domain**: entidades, erros, resultados, eventos e contratos do domínio.
- **Application**: casos de uso com MediatR, comportamentos e handlers.
- **Infrastructure**: scraper Playwright, persistência SQL Server, worker, Discord e integração externa.
- **API**: configuração do ASP.NET Core, versionamento e controllers.
- **Tests**: testes unitários e de integração do scraper, repositório e handlers.
- **frontend**: interface web para pesquisa, paginação e detalhes das vagas.

O fluxo principal é:

1. O `ScrapperWorkerService` executa a coleta a cada minuto.
2. O command `ScrapeJobsCommand` envia os termos para todos os scrapers registrados.
3. O `LinkedinJobScrapper` abre o LinkedIn em Chromium e extrai título, empresa e URL.
4. O repositório remove vagas duplicadas e persiste somente as novas.
5. O evento `ScrapingFinishedEvent` dispara o envio das vagas ao Discord.

## Pré-requisitos

### Execução com Docker

- Docker Desktop
- Docker Compose

### Execução local

- SDK .NET 10
- Node.js 20.19+ ou 22.12+
- SQL Server disponível localmente ou em container
- Navegadores do Playwright instalados

## Configuração

Crie um arquivo `.env` na raiz do projeto para o Docker Compose:

```dotenv
DB_NAME=CareerSearcher
DB_PASSWORD=UmaSenhaForteComCaracteresEspeciais!
WEBHOOK_URL=https://discord.com/api/webhooks/SEU_ID/SEU_TOKEN
```

| Variável      | Descrição                                       |
| ------------- | ----------------------------------------------- |
| `DB_NAME`     | Nome do banco SQL Server criado pela aplicação. |
| `DB_PASSWORD` | Senha da conta `sa` usada pelo SQL Server.      |
| `WEBHOOK_URL` | URL completa de um webhook do Discord.          |

## Executando com Docker

Na raiz do projeto:

```powershell
docker compose up -d --build
```

Os serviços disponibilizados são:

| Serviço    | Endereço                      |
| ---------- | ----------------------------- |
| API        | http://localhost:8080         |
| Swagger UI | http://localhost:8080/swagger |
| Frontend   | http://localhost:5173         |
| SQL Server | `localhost:1433`              |

## Executando localmente

1. Instale o SDK .NET 10.
2. Configure uma conexão SQL Server.
3. Exporte as configurações necessárias:

```powershell
$env:ConnectionStrings__SqlConnection = "Server=localhost,1433;Database=CareerSearcher;User ID=sa;Password=SuaSenha;TrustServerCertificate=True"
$env:WebhookUrl = "https://discord.com/api/webhooks/SEU_ID/SEU_TOKEN"
```

4. Restaure e instale o Chromium do Playwright:

```powershell
dotnet restore
dotnet build
pwsh src/CareerSearcher.Infrastructure/bin/Debug/net10.0/playwright.ps1 install chromium
```

5. Inicie a API:

```powershell
dotnet run --project src/CareerSearcher.API
```

Por padrão, o perfil `http` inicia em `http://localhost:5043`.

### Executando o frontend

Com a API em execução em `http://localhost:5043`, abra outro terminal:

```powershell
cd frontend
npm install
npm run dev
```

O Vite inicia a interface em `http://localhost:5173` e encaminha as chamadas de
`/api` para a API local. Para usar outro endereço durante o desenvolvimento,
copie `frontend/.env.example` para `frontend/.env` e ajuste `API_URL`.
Em uma implantação sem proxy de mesma origem, configure `VITE_API_BASE_URL`
com a URL base da API (por exemplo, `https://api.example.com/api/v1`) e permita
a origem do frontend na configuração de CORS da API.

## API

Todas as rotas de versão 1 usam o prefixo:

```text
/api/v1
```

### Listar vagas

```http
GET /api/v1/jobs?page=1&pageSize=10
```

Parâmetros:

| Parâmetro  | Tipo  | Padrão | Descrição                                                                      |
| ---------- | ----- | -----: | ------------------------------------------------------------------------------ |
| `page`     | int   |    `1` | Página solicitada. Valores menores ou iguais a zero são normalizados para `1`. |
| `pageSize` | int   |   `10` | Quantidade de vagas por página.                                                |
| `search`   | texto |        | Busca em título, empresa e descrição, antes da paginação.                      |

A resposta contém metadados de paginação e os registros de vagas:

```json
{
  "page": 1,
  "pageSize": 10,
  "amount": 30,
  "pages": 3,
  "totalInPage": 1,
  "data": [
    {
      "name": "Desenvolvedor .NET",
      "company": "Empresa",
      "description": "",
      "url": "https://...",
      "uniqueId": "Desenvolvedor .NET+Empresa"
    }
  ]
}
```

### Versionamento

Além da versão na URL, a API aceita o header:

```http
X-Api-Version: 1
```

O Swagger pode ser acessado na raiz da aplicação. Em containers, por exemplo:

- Swagger UI: `http://localhost:8080/swagger`
- OpenAPI JSON: `http://localhost:8080/swagger/v1/swagger.json`

## Worker e scraping

Ao iniciar, a API registra o `ScrapperWorkerService`. O worker:

- executa imediatamente;
- repete a coleta a cada minuto;
- processa três consultas padrão;
- usa um contexto Chromium compartilhado;
- publica um evento após concluir cada ciclo.

O scraper consulta a página pública de vagas do LinkedIn por exemplo, e extrai os elementos:

- título: `h3`
- empresa: `h4`
- URL: primeiro link do item

Alguns sites podem alterar seu HTML ou bloquear coleta automatizada. Portanto, eventuais mudanças no layout ou respostas de página podem exigir ajustes nos seletores.

## Testes

O projeto contém testes para:

- criação de navegador Chromium;
- scraping do LinkedIn;
- persistência de vagas;
- handler de scraping;
- construção do payload do Discord.

Execute:

```powershell
dotnet test
```

Alguns testes dependem de recursos externos, como o LinkedIn, e podem ser afetados por conectividade ou bloqueios do site.
