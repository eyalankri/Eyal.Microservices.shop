
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Members.Entities;
using Common.MongoDB;
using Common.MassTransit;
using Common.Jwt;
using Microsoft.Extensions.Configuration;
using Common.Settings;

var builder = WebApplication.CreateBuilder(args);




var jwtConfig = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();


// Services is IServiceCollection
builder.Services
        .AddMongo()
        .AddMongoRepository<User>("User") // add `User` table to the db name declared in appsettings
        .AddMassTransitWithRabbitMq()
        .AddJwtAuthentication(jwtConfig!);



builder.Services.AddControllers();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// add the option to enter token in Swagger (lock icon)
builder.Services.AddSwaggerGen();


////JWT
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
//        ValidateAudience = true,
//        ValidAudience = builder.Configuration["JwtSettings:Audience"],
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!))
//    };
//});


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
