using StickedWords.API;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogger();
builder.Services.AddHealthChecks();
builder.AddServices();
builder.Services.ConfigureHttp();

var app = builder.Build();

if (app.Configuration.GetValue("ApplyMigrations", false))
{
    app.Services.ApplyMigrations();
}

app.UseHttpsRedirection();
app.RegisterSpa();

app.RegisterEndpoints();

app.MapHealthChecks("/health");

app.Run();
