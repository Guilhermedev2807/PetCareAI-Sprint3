# PetCareAI - API de Gestão de Pets 🐾

API RESTful desenvolvida em **ASP.NET Core 8** para o gerenciamento de pets, serviços de cuidados e agendamentos de consultas com assistente IA integrado. Projeto desenvolvido para a avaliação da **Sprint 3** do curso de Análise e Desenvolvimento de Sistemas.

---

## 👥 Integrantes do Grupo

* **Bruno Vinicius Barbosa** - RM 566366
* **Guilherme de Andrade Martini** - RM 566087
* **Nathan Gonçalves Pereira Mendes** - RM 564666
* **Raphael Gomes Mancera** - RM 562279

---

## 📌 Descrição do Projeto

O **PetCareAI** é uma solução para clínicas veterinárias e tutores de animais de estimação. A aplicação centraliza o histórico de atendimento dos pets, agendamentos de consultas e recomendações de saúde personalizadas baseadas em inteligência artificial.

### 💡 Benefícios para o Negócio

Para a clínica veterinária, ter o histórico de pets e consultas centralizado e relacionado elimina a dependência de fichas físicas ou planilhas soltas — qualquer atendimento pode consultar rapidamente o histórico completo de um animal, sem retrabalho. Para o tutor, isso significa atendimentos mais rápidos e menos repetição de informação a cada visita. Por rodar em nuvem, a solução também fica acessível de qualquer lugar da clínica, sem depender de um servidor local.

### 📜 Principais Funcionalidades
* **CRUD Completo de Pets e Consultas**: Cadastro, listagem, atualização e remoção de registros de animais e históricos de consultas.
* **Persistência de Dados Relacional**: Mapeamento objeto-relacional com **Entity Framework Core** conectado ao banco de dados **Azure SQL**.
* **Testes Automatizados**: Suíte de testes unitários com **xUnit**, **Moq** e **FluentAssertions**.
* **Observabilidade e Logs**: Monitoramento via **Serilog**, **OpenTelemetry** e **Health Checks** embutidos para verificação de status do serviço.

---

## 🔄 Mudanças e Atualizações (Sprint 3)

Nesta entrega, foram implementadas as seguintes melhorias técnicas na arquitetura da solução:

1. **Migração para Azure SQL**: Conexão e script de criação do banco de dados relacional na nuvem Microsoft Azure.
2. **Nova Entidade `Consulta`**: Criação de endpoints e repositórios para gerenciamento de consultas veterinárias vinculadas aos pets.
3. **Suíte de Testes com xUnit**:
   * Implementação de testes unitários para a camada de serviço (`PetServiceTests`).
   * Adição dos pacotes `FluentAssertions` para asserções fluídas e `Moq` para isolamento de dados.
4. **Padronização do Repositório**: Reestruturação da raiz do projeto (`src`, `tests`, `.sln`) para integração contínua e avaliação limpa.
5. **Observabilidade Ampliada**: Adição do endpoint `/health` para validação de saúde do ecossistema e suporte a exportação de métricas OpenTelemetry.

---

## 🛠️ Tecnologias Utilizadas

* **Linguagem / Framework**: C# 12 | .NET 8.0 (ASP.NET Core Web API)
* **Banco de Dados**: Azure SQL Server / Entity Framework Core 8
* **Testes**: xUnit, Moq, FluentAssertions
* **Logs & Telemetria**: Serilog, OpenTelemetry, HealthChecks
* **Documentação de API**: Swagger / OpenAPI

---

## 🚀 Como Executar o Projeto

### Pré-requisitos
* [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) instalado.
* IDE (Visual Studio 2022, VS Code ou Rider).

### Passos de Execução

1. **Clonar o repositório:**
   ```bash
   git clone https://github.com/Guilhermedev2807/PetCareAI-Sprint3.git
   cd PetCareAI-Sprint3
   ```

2. **Restaurar as dependências do projeto:**
   ```bash
   dotnet restore PetCareSprint3C.sln
   ```

3. **Executar a suíte de testes unitários:**
   ```bash
   dotnet test tests/PetCareAI.Tests.Unit/PetCareAI.Tests.Unit.csproj
   ```

4. **Executar a API:**
   ```bash
   dotnet run --project src/PetCareAI.Api/PetCareAI.Api.csproj
   ```

5. **Acessar a documentação no navegador:**
   Navegue até `http://localhost:5000/swagger` ou `https://localhost:7000/swagger`.

---

## ☁️ Infraestrutura em Nuvem (Azure)

A aplicação roda em um **Azure App Service** (Linux, sem containers), conectado a um **Azure SQL Database** (PaaS), ambos provisionados via Azure CLI.

### Passo a passo de deploy

1. Provisione os recursos na Azure (Resource Group, Azure SQL Server, banco, firewall, App Service Plan e Web App) via Azure CLI.
2. Publique o build da aplicação especificamente para Linux:
   ```powershell
   dotnet publish -c Release -r linux-x64 --self-contained false -o ./publish
   ```
3. Compacte o resultado publicado e envie para o App Service:
   ```powershell
   Add-Type -AssemblyName System.IO.Compression.FileSystem
   [System.IO.Compression.ZipFile]::CreateFromDirectory((Resolve-Path ./publish).Path, (Join-Path (Get-Location) "publish.zip"))
   az webapp deploy --resource-group <nome-do-resource-group> --name <nome-do-webapp> --src-path ./publish.zip --type zip --clean true
   ```
4. As migrations do Entity Framework são aplicadas automaticamente ao iniciar a aplicação, criando as tabelas `Pets` e `Consultas` no Azure SQL.
5. Acesse `https://<nome-do-webapp>.azurewebsites.net/swagger` para confirmar que a API está no ar.

---
