using DocumentManagementSystem.Controllers;
using DocumentManagementSystem.DataAccess;
using DocumentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IDocumentRepository, DocumentRepository>();
builder.Services.AddDbContextPool<DatabaseContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MainDatabase") ??
        throw new InvalidOperationException("Connection string 'MainDatabase'" +" not found.")));


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
