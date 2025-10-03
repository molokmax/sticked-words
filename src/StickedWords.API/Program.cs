using Prometheus;
using StickedWords.API;
using StickedWords.Background;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogger();
builder.Services.AddHealthChecks();
builder.Services.UseHttpClientMetrics();
builder.Services.ConfigureHttp();

builder.AddServices();

var app = builder.Build();

if (app.Configuration.GetValue("ApplyMigrations", false))
{
    app.Services.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseHttpMetrics();
app.MapHealthChecks("/health");
app.MapMetrics("/metrics");
app.UseBackground();
app.RegisterSpa();
app.RegisterEndpoints();

app.Run();
