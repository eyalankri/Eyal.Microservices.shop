using Common.MassTransit;
using Common.MongoDB;
using Orders.Clients;
using Orders.Entities;
using Polly;
using Polly.Timeout;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.



        // Services is IServiceCollection
        builder.Services
                .AddMongo()
                .AddMongoRepository<Order>("orders") // add `orders` table to the db name declared in appsettings
                .AddMongoRepository<Product>("products") // add `orders` table to the db name declared in appsettings
                .AddMassTransitWithRabbitMq();



        AddCatalogClient(builder.Services);

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
    }

    private static void AddCatalogClient(IServiceCollection services)
    {
        Random jitterer = new Random();

        services.AddHttpClient<ProductClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7270");
        })
            // Add a transient HTTP error policy that will retry for 5 times, waiting for 5 seconds between retries, for the following errors:
                // * HttpRequestException
                // * TimeoutRejectedException
        .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>()
            .WaitAndRetryAsync(
                5,retryAttempt => TimeSpan.FromSeconds(5)
        ))
            // Add a transient HTTP error policy that will open the circuit for 15 seconds after 3 consecutive failures for the following errors:
                // * HttpRequestException
                // * TimeoutRejectedException
        .AddTransientHttpErrorPolicy(builder => builder.Or<TimeoutRejectedException>().CircuitBreakerAsync(
            3,
            TimeSpan.FromSeconds(15)            
        ))
            // Add a policy that will timeout HTTP requests after 1 second.
        .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
    }

}