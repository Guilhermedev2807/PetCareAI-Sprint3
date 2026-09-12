# PetCareAI - API de Gestão de Pets 🐾

> **FIAP - Faculdade de Informática e Administração Paulista**  
> **Disciplina:** Advanced Business Development with .NET  
> **Sprint 3**

---

## 👥 Integrantes do Grupo

- **Guilherme de Andrade Martini** – RM 566087
- **Nathan Gonçalves Pereira Mendes** – RM 564666
- **Raphael Gomes Mancera** – RM 562279
- **Bruno Vinicius Barbosa** – RM 566366


---

## 📌 Sobre o Projeto

O **PetCareAI** é uma solução para o gerenciamento inteligente de pets e assistência à saúde animal. Nesta **3ª Sprint**, a aplicação ASP.NET Core foi evoluída com foco em **Monitoramento, Observabilidade e Testes Automatizados**, garantindo alta resiliência, rastreabilidade e qualidade de código seguindo as melhores práticas do mercado.

---

## 🛠️ Tecnologias e Recursos

- **.NET 8 / ASP.NET Core API**
- **Entity Framework Core** (In-Memory Database)
- **Swagger / OpenAPI** (Documentação interativa das rotas)
- **Serilog** (Logging Estruturado gravado em Console e Arquivos)
- **Health Checks** (`Microsoft.Extensions.Diagnostics.HealthChecks`)
- **OpenTelemetry** (Tracing distribuído e coleta de métricas)
- **xUnit, Moq & WebApplicationFactory** (Suíte de Testes Unitários e de Integração — Padrão AAA)

---

## 📁 Estrutura da Solução

```text
PetCareSprint3C#/
│
├── src/
│   └── PetCareAI.Api/               # Projeto Principal da API (Controllers, Services, Models)
│
├── tests/
│   ├── PetCareAI.Tests.Unit/        # Testes Unitários de Domínio e Serviços (xUnit + Moq)
│   └── PetCareAI.Tests.Integration/ # Testes de Integração de Endpoints (WebApplicationFactory)
│
└── PetCareSprint3C.sln              # Arquivo de Solução do Projeto
🚀 Como Executar a Aplicação
Pré-requisitos
.NET 8.0 SDK instalado.

Passo a passo
Clone ou baixe o repositório.

Abra o terminal na pasta raiz da solução (PetCareSprint3C#).

Execute o comando para iniciar a API no ambiente de desenvolvimento:

PowerShell
$env:ASPNETCORE_ENVIRONMENT="Development"; dotnet run --project src/PetCareAI.Api/PetCareAI.Api.csproj
A API estará acessível em: http://localhost:5000

📊 Endpoints de Monitoramento e Observabilidade
Health Check: GET http://localhost:5000/health

Verifica a saúde da aplicação, conexões e disponibilidade do serviço.

Swagger UI: GET http://localhost:5000/swagger

Interface interativa para documentação, teste de rotas e schemas dos endpoints.

🧪 Como Executar os Testes Automatizados
A suíte de testes cobre a camada de serviços (unidade) e os fluxos HTTP dos endpoints (integração), utilizando o padrão AAA (Arrange, Act, Assert) e nomenclaturas padronizadas (MetodoTestado_Cenario_ResultadoEsperado).

Para executar todos os testes da solução, rode o seguinte comando no terminal:

PowerShell
dotnet test PetCareSprint3C.sln
Para rodar os projetos de teste separadamente:

Apenas Testes Unitários:

PowerShell
dotnet test tests/PetCareAI.Tests.Unit/PetCareAI.Tests.Unit.csproj
Apenas Testes de Integração:

PowerShell
dotnet test tests/PetCareAI.Tests.Integration/PetCareAI.Tests.Integration.csproj

---

### Passo a Passo para Atualizar no GitHub:

1. Abra o arquivo `README.md` no seu VS Code, cole o conteúdo acima e salve.
2. Abra o terminal e execute os comandos para atualizar o GitHub:

```powershell
git add README.md
git commit -m "docs: adiciona nomes e RMs do grupo no README"
git push origin main
