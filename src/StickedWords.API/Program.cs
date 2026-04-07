using Prometheus;
using StickedWords.API;
using StickedWords.Background;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogger();
builder.AddServiceDefaults();
builder.Services.UseHttpClientMetrics();
builder.Services.ConfigureHttp();
builder.Services.AddHttpContextAccessor();
builder.AddAuth();

builder.AddServices();

var app = builder.Build();

if (app.Configuration.GetValue("ApplyMigrations", false))
{
    app.Services.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapDefaultEndpoints();
app.UseHttpMetrics();
app.MapMetrics("/metrics");
app.UseBackground();
app.RegisterSpa();
app.RegisterEndpoints();

app.Run();
