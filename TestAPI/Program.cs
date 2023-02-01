using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using WebAPI.Logic;
using WebAPI.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
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

using (ApplicationContext db = new ApplicationContext())
{
    //Developer dev = new Developer { Name = "sdadasd", RegistrationDate = DateTime.UtcNow, LogoURL = "d" };
    //db.Developers.AddRange(dev);
    //db.Database.ExecuteSqlRaw("DROP TABLE Developers");
    //db.Database.ExecuteSqlRaw("DROP TABLE Games");
    //db.Database.ExecuteSqlRaw("DROP TABLE [Users]");
    //db.Database.ExecuteSqlRaw("DROP TABLE [GamesStats]");
    //db.Database.ExecuteSqlRaw("DROP TABLE [Reviews]");
    db.SaveChanges();
}

app.Run();

