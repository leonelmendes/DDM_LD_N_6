using Microsoft.EntityFrameworkCore;
using iTaskAPI.Connection;
using iTaskAPI.Repository.TarefaRepository;
using iTaskAPI.Repository.GestorRepository;
using iTaskAPI.Repository.ProgramadorRepository;
using iTaskAPI.Repository.TipoTarefaRepository;
using iTaskAPI.Repository.UtilizadorRepository;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using iTaskAPI.Repository.AuthRepository;
using System.Text.Json.Serialization;
//using iTaskAPI.Repository.Interfaces;
//using iTaskAPI.Repository.Implementations;

var builder = WebApplication.CreateBuilder(args);

//config database connection
builder.Services.AddDbContext<ConnectionDB>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PgConnection"))
);

// INJEÇÃO DE DEPENDÊNCIA DOS REPOSITÓRIOS
// Aqui registramos os repositórios que serão usados nos controllers
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<IGestorRepository, GestorRepository>();
builder.Services.AddScoped<IProgramadorRepository, ProgramadorRepository>();
builder.Services.AddScoped<ITipoTarefaRepository, TipoTarefaRepository>();
builder.Services.AddScoped<IUtilizadorRepository, UtilizadorRepository>();
builder.Services.AddScoped<IAuthenticateRepository, AuthenticateRepository>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
     {
         // Esta linha impede o loop infinito se houver objetos aninhados
         options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
         // Opcional: formata o JSON bonito para leitura
         options.JsonSerializerOptions.WriteIndented = true;
     });

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// teste conexao Console.WriteLine("Connection: " + builder.Configuration.GetConnectionString("PgConnection"));

//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();