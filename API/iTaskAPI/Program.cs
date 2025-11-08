using Microsoft.EntityFrameworkCore;
using iTaskAPI.Connection;
//using iTaskAPI.Repository.Interfaces;
//using iTaskAPI.Repository.Implementations;

var builder = WebApplication.CreateBuilder(args);

//config database connection
builder.Services.AddDbContext<ConnectionDB>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PgConnection"))
);

builder.Services.AddControllers();

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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();