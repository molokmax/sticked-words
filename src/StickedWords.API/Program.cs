using StickedWords.API;
using StickedWords.API.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddLogger();
builder.Services.AddServices();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.RegisterFlashCardEndpoints();

app.Run();
