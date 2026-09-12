using Serilog;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using PetCareAI.Infrastructure.Context;
using PetCareAI.Domain.Interfaces;
using PetCareAI.Application.Services;


var builder = WebApplication.CreateBuilder(args);

// 1. Configuração do Serilog (Logging Estruturado)
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithProperty("ApplicationName", "PetCareAI.Api")
    .WriteTo.Console()
    .WriteTo.File("logs/petcareai-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// 2. Injeção de Dependências, Banco de Dados e Serviços
// Banco em nuvem (Azure SQL) — a connection string vem do appsettings.json
// localmente, e da configuração do App Service (az webapp config connection-string set)
// quando publicado na Azure.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPetRepository, PetRepository>();
builder.Services.AddScoped<PetService>();
builder.Services.AddScoped<IConsultaRepository, ConsultaRepository>();
builder.Services.AddScoped<ConsultaService>();

// 3. Configuração do Health Check
builder.Services.AddHealthChecks()
    .AddCheck("Self", () => HealthCheckResult.Healthy("API PetCareAI operacional."))
    .AddDbContextCheck<AppDbContext>("Database");

// 4. Configuração do OpenTelemetry (Tracing e Métricas)
builder.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing
            .AddSource("PetCareAI.Api")
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddConsoleExporter();
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddPrometheusExporter();
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CRIAÇÃO DA APLICAÇÃO ---
var app = builder.Build();

// 5. Configuração do Pipeline HTTP (Sempre após o app = builder.Build())
app.UseSerilogRequestLogging();

// Swagger habilitado sempre (inclusive em Production/App Service), pois a
// entrega de DevOps exige demonstrar os endpoints via vídeo contra o app publicado.
app.UseSwagger();
app.UseSwaggerUI();

// Aplica migrations pendentes automaticamente ao iniciar (não em Testing)
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        db.Database.Migrate();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex,
            "Falha ao aplicar migrations. Rode 'dotnet ef migrations add InitialCreate' " +
            "na pasta do projeto antes de iniciar a API, e confira a ConnectionString " +
            "em appsettings.json.");
        throw;
    }
}

// Endpoint do Health Check registrado corretamente no pipeline
app.MapHealthChecks("/health");

app.UseAuthorization();
app.MapControllers();

app.Run();

public partial class Program { }
