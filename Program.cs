using Hotelaria.Services;
using Hotelaria.HealthChecks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// CONFIGURAÇÃO DE PORTAS PARA AZURE
// ==========================================
var port = Environment.GetEnvironmentVariable("PORT") ?? 
           Environment.GetEnvironmentVariable("WEBSITES_PORT") ?? "8080";

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

// Logging estruturado
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configurar níveis de log
if (builder.Environment.IsDevelopment())
{
    builder.Logging.SetMinimumLevel(LogLevel.Debug);
}
else
{
    builder.Logging.SetMinimumLevel(LogLevel.Information);
}

// Log de inicialização
var logger = LoggerFactory.Create(config => config.AddConsole()).CreateLogger("Startup");
logger.LogInformation("🚀 Iniciando Sistema Hotelaria v2.6.2");
logger.LogInformation($"📦 Environment: {builder.Environment.EnvironmentName}");
logger.LogInformation($"🌐 Port: {port}");

// ==========================================
// VALIDAÇÃO DE VARIÁVEIS DE AMBIENTO
// ==========================================
var mongoUri = builder.Configuration["MONGO_ATLAS_URI"] ?? 
               builder.Configuration["MONGO_CONNECTION_STRING"] ??
               Environment.GetEnvironmentVariable("MONGO_ATLAS_URI") ??
               Environment.GetEnvironmentVariable("MONGO_CONNECTION_STRING");

if (string.IsNullOrEmpty(mongoUri))
{
    logger.LogWarning("⚠️  MONGO_ATLAS_URI não configurado. MongoDB será executado em modo fallback.");
}
else
{
    logger.LogInformation("✅ MongoDB configurado");
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Health Checks AVANÇADOS
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("Application is running"))
    .AddCheck<MongoDbHealthCheck>("mongodb", tags: new[] { "database", "mongodb" })
    .AddCheck<PayPalHealthCheck>("paypal", tags: new[] { "integration", "payment" })
    .AddCheck<AirbnbHealthCheck>("airbnb", tags: new[] { "integration", "booking" })
    .AddCheck<MemoryHealthCheck>("memory", tags: new[] { "system", "resources" });

// Services
builder.Services.AddSingleton<ReservaService>();
builder.Services.AddSingleton<QuartoService>();
builder.Services.AddSingleton<HospedeService>();
builder.Services.AddSingleton<ImportacaoService>();
builder.Services.AddSingleton<RelatorioService>();

builder.Services.AddSingleton<ConfigurationService>();
builder.Services.AddSingleton<AuditService>();

// HttpClient com Polly para resiliência
builder.Services.AddHttpClient<PayPalService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "Hotelaria/2.6.2");
});

builder.Services.AddHttpClient<AirbnbService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "Hotelaria/2.6.2");
});

builder.Services.AddHttpClient<SentryService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "Hotelaria/2.6.2");
});

// Ollama Service para IA
builder.Services.AddHttpClient<OllamaService>(client =>
{
    client.Timeout = TimeSpan.FromMinutes(5); // Maior timeout para IA
    client.DefaultRequestHeaders.Add("User-Agent", "Hotelaria/2.6.2");
});
logger.LogInformation("✅ OllamaService registrado");

// MongoDB Service com tratamento de erro
try
{
    builder.Services.AddSingleton<MongoDBService>();
    logger.LogInformation("✅ MongoDBService registrado");
}
catch (Exception ex)
{
    logger.LogWarning($"⚠️  Erro ao registrar MongoDBService: {ex.Message}");
}

// Antiforgery (CSRF Protection)
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    
    // Sempre usar Secure em produção, apenas permitir sem Secure em dev local
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
});

var app = builder.Build();

// Log de build completo
logger.LogInformation("✅ Aplicação construída com sucesso");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    
    // HSTS apenas se não estiver atrás de um proxy (Azure tem seu próprio HTTPS)
    var behindProxy = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME") != null;
    if (!behindProxy)
    {
        app.UseHsts();
    }
    
    logger.LogInformation($"🔒 Production mode. Behind proxy: {behindProxy}");
}

// Security Headers
app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Append("Referrer-Policy", "strict-origin-when-cross-origin");
    
    // CSP mais permissivo para Blazor funcionar corretamente
    context.Response.Headers.Append("Content-Security-Policy", 
        "default-src 'self'; " +
        "script-src 'self' 'unsafe-inline' 'unsafe-eval' https:; " +
        "style-src 'self' 'unsafe-inline' https:; " +
        "img-src 'self' data: https:; " +
        "font-src 'self' data:; " +
        "connect-src 'self' wss: https:; " +
        "frame-ancestors 'none';");
    
    await next();
});

// Health check endpoint COMPLETO (CRÍTICO PARA AZURE)
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            status = report.Status.ToString(),
            timestamp = DateTime.UtcNow,
            duration = report.TotalDuration.ToString(),
            checks = report.Entries.Select(x => new
            {
                name = x.Key,
                status = x.Value.Status.ToString(),
                description = x.Value.Description,
                duration = x.Value.Duration.ToString(),
                data = x.Value.Data,
                tags = x.Value.Tags
            }),
            version = "2.6.2",
            environment = app.Environment.EnvironmentName
        };
        
        await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
    }
});

// Health check por categoria
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("database") || check.Tags.Contains("auth"),
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var result = new { status = report.Status.ToString(), checks = report.Entries.Count };
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = _ => false, // Apenas check básico
    ResponseWriter = async (context, report) =>
    {
        await context.Response.WriteAsync(JsonSerializer.Serialize(new { status = "alive" }));
    }
});

// Endpoint de diagnóstico simples
app.MapGet("/api/status", () => Results.Ok(new
{
    status = "running",
    version = "2.6.2",
    environment = app.Environment.EnvironmentName,
    timestamp = DateTime.UtcNow,
    uptime = Environment.TickCount64 / 1000 // segundos
}));

// Endpoint de métricas (apenas em dev)
if (app.Environment.IsDevelopment())
{
    app.MapGet("/api/metrics", () => Results.Ok(new
    {
        memory = new
        {
            allocated = GC.GetTotalMemory(false),
            gen0 = GC.CollectionCount(0),
            gen1 = GC.CollectionCount(1),
            gen2 = GC.CollectionCount(2)
        },
        process = new
        {
            threads = Environment.ProcessorCount,
            workingSet = Environment.WorkingSet
        }
    }));
}

// HTTPS Redirect apenas em desenvolvimento local
// No Azure, o App Service já gerencia HTTPS
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

logger.LogInformation($"🌐 Aplicação pronta. Listening on port {port}");
logger.LogInformation($"🏥 Health check disponível em: /health");
logger.LogInformation($"🏥 Readiness check disponível em: /health/ready");
logger.LogInformation($"🏥 Liveness check disponível em: /health/live");
logger.LogInformation($"📊 Status endpoint disponível em: /api/status");

app.Run();
