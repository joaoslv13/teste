using MediatR;
using Microsoft.Data.Sqlite;
using Questao5.Infrastructure.Database.Repositories;
using Questao5.Infrastructure.Sqlite;
using System.Data;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddTransient<IMovimentoRepository, MovimentoRepository>();
builder.Services.AddTransient<IContaCorrenteRepository, ContaCorrenteRepository>();

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddSingleton(new DatabaseConfig { Name = builder.Configuration.GetValue<string>("DatabaseName", "Data Source=database.sqlite") });
builder.Services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IDbConnection>(provider =>
{
    var config = provider.GetRequiredService<DatabaseConfig>();
    return new SqliteConnection(config.Name);
});




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Services.GetRequiredService<IDatabaseBootstrap>().Setup();

app.Run();
