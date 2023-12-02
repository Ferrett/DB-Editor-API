using Amazon.S3.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using WebAPI.Logic;
using WebAPI.Models;
using WebAPI.Services.S3Bucket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using WebAPI.Services.Validation.UserValidation;
using WebAPI.Services.Validation.DeveloperValidation;
using WebAPI.Services.Validation.GameValidation;
using WebAPI.Services.Validation.GameStatsValidation;
using WebAPI.Services.Validation.ReviewValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;
using System.IdentityModel.Tokens;
using WebAPI.Services.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<ImageUpload>()
    .AddClasses(classes => classes.AssignableTo<IS3Bucket>())
    .AsImplementedInterfaces()
    .WithTransientLifetime());

builder.Services.AddScoped<IUserValidation, UserValidation>();
builder.Services.AddScoped<IDeveloperValidation, DeveloperValidation>();
builder.Services.AddScoped<IGameValidation, GameValidation>();
builder.Services.AddScoped<IReviewValidation, ReviewValidation>();
builder.Services.AddScoped<IGameStatsValidation, GameStatsValidation>();
builder.Services.AddScoped<IUserAuthenticationService, UserAuthenticationService>();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:SecretKey"]);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuerSigningKey = true
    };
});
    
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();

