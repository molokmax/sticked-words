using Microsoft.Extensions.Configuration;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var path = builder.Configuration.GetValue<string>("SQLite:Path");
var fileName = builder.Configuration.GetValue<string>("SQLite:FileName");
var db = builder.AddSqlite("sqlite", path, fileName);

var api = builder.AddProject<StickedWords_API>("api")
    .WithHttpHealthCheck("/health")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
