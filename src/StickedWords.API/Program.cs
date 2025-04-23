using StickedWords.API;
using StickedWords.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogger();
builder.Services.AddHealthChecks();
builder.Services.AddServices();

var app = builder.Build();

if (app.Configuration.GetValue("ApplyMigrations", false))
{
    app.Services.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.RegisterFlashCardEndpoints();

app.MapHealthChecks("/health");

app.Run();
