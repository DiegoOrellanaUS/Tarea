using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("SistemaContext")
    ?? new NpgsqlConnectionStringBuilder
    {
        Host = builder.Configuration["POSTGRES_HOST"] ?? "localhost",
        Database = builder.Configuration["POSTGRES_DB"] ?? "sistema",
        Username = builder.Configuration["POSTGRES_USER"] ?? "sistema",
        Password = builder.Configuration["POSTGRES_PASSWORD"]
    }.ConnectionString;

builder.Services.AddDbContext<SistemaContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var database = scope.ServiceProvider.GetRequiredService<SistemaContext>();
    await database.Database.MigrateAsync();
}

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
