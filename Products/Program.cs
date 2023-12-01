using Common.Jwt;
using Common.MassTransit;
using Common.MongoDB;
using Common.Settings;
using Products.Entities;

var builder = WebApplication.CreateBuilder(args);



var jwtConfig = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();


builder.Services
        .AddMongo()
        .AddMongoRepository<Product>("items") // add `items` table to the db name declared in appsettings
        .AddMassTransitWithRabbitMq()
        .AddJwtAuthentication(jwtConfig!);



                

// Add services to the container.

builder.Services.AddControllers();
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
