using Hotelaria.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Services
builder.Services.AddSingleton<ReservaService>();
builder.Services.AddSingleton<QuartoService>();
builder.Services.AddSingleton<HospedeService>();
builder.Services.AddSingleton<ImportacaoService>();
builder.Services.AddSingleton<RelatorioService>();

// Repositório de usuários compartilhado (Singleton)
builder.Services.AddSingleton<UserRepository>();

// AuthService isolado por sessão (Scoped) - CORREÇÃO DE SEGURANÇA CRÍTICA
builder.Services.AddScoped<AuthService>();

builder.Services.AddSingleton<ConfigurationService>();
builder.Services.AddSingleton<AuditService>();

// HttpClient com Polly para resiliência
builder.Services.AddHttpClient<PayPalService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "Hotelaria/2.6.0");
});

builder.Services.AddHttpClient<AirbnbService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "Hotelaria/2.6.0");
});

builder.Services.AddHttpClient<SentryService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "Hotelaria/2.6.0");
});

builder.Services.AddSingleton<MongoDBService>();

// Antiforgery (CSRF Protection)
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Cookie.HttpOnly = true;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
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

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
