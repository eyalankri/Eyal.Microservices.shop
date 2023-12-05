using Members.Entities;
using Common.MongoDB;
using Common.MassTransit;
using Common.Jwt;
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

 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
