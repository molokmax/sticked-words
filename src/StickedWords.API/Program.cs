using Prometheus;
using StickedWords.API;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogger();
builder.Services.AddHealthChecks();

builder.Services.UseHttpClientMetrics();

builder.AddServices();
builder.Services.ConfigureHttp();

var app = builder.Build();

if (app.Configuration.GetValue("ApplyMigrations", false))
{
    app.Services.ApplyMigrations();
}

app.UseHttpsRedirection();
app.RegisterSpa();

app.UseHttpMetrics();

app.RegisterEndpoints();

app.MapMetrics("/metrics");

app.MapHealthChecks("/health");

app.Run();
