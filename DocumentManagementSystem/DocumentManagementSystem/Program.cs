using DocumentManagementSystem.Controllers;
using DocumentManagementSystem.DataAccess;
using DocumentManagementSystem.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;
using Messaging.Common;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Messaging.Common.Options;
using Messaging.Common.Extensions;
using DocumentManagementSystem.Messaging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
builder.Services.AddDbContextPool<DatabaseContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("MainDatabase") ??
        throw new InvalidOperationException("Connection string 'MainDatabase'" +" not found.")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

//RabbitMQ aufgebaut mithilfe von https://dotnettutorials.net
var mq = builder.Configuration.GetSection("RabbitMq").Get<RabbitMqOptions>()!;
builder.Services.AddRabbitMq(mq.HostName, mq.UserName, mq.Password, mq.VirtualHost);
builder.Services.AddSingleton<INewDocumentPublisher, NewDocumentPublisher>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
    db.Database.Migrate();
}


app.Run();
