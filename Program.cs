
using Azure.Identity;
using cosmosdb_test.Interfaces;
using cosmosdb_test.Repositories;
using cosmosdb_test.Repositories.Context;
using cosmosdb_test.Repositories.Interfaces;
using cosmosdb_test.Services;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace cosmosdb_test;

// Implemented: 
// Cosmos and SQL example context with services.
// Integrated KeyVault secret extraction
// SQL is setup with strict security requiring authentication through tenantID.

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Azure Key Vault to securely retrieve connection strings and keys
        // DefaultAzureCredential automatically handles authentication in Azure
        // (uses Managed Identity in production, VS/Azure CLI credentials locally
        builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault:Name"]!), new DefaultAzureCredential());

        // SQL uses Managed Identity authentication (connection string includes Authentication=Active Directory Default when running locally and different string when running in production environment)
        // Cosmos uses connection string authentication (AccountKey in connection string)
        var sqlConnectionString = builder.Environment.IsProduction() ? builder.Configuration["SQLPRODUCTION"]!  : builder.Configuration["SQLCONNECTIONSTRING"]!;

        // Setting up logging with Apllication insights.
        builder.Services.AddApplicationInsightsTelemetry(options =>
        {
            options.ConnectionString = builder.Configuration["APPLICATIONINSIGHT"];
        });

        // Add services to the container.
        builder.Services.AddControllers();

        // Add database context, using the retrieved secrets from keyvault.
        builder.Services.AddDbContext<CosmosContext>(options => options.UseCosmos(builder.Configuration["COSMOSENDPOINT"]!, builder.Configuration["COSMOSKEY"]!, databaseName: "GameDB"));
        builder.Services.AddDbContext<SqlContext>(options => options.UseSqlServer(sqlConnectionString));

        // Add services
        builder.Services.AddOpenApi();

        // Mapster automatically maps between objects with similar property names
        // Example: IGameState → PlayedGame, PlayedGame → GameStateDto
        builder.Services.AddMapster();

        // Set up our services.
        builder.Services.AddTransient<IGameState, GameStateService>(); // transient => new service each request
        builder.Services.AddScoped<ICosmosRepository, CosmosRepository>();
        builder.Services.AddScoped<ISqlRepository, SqlRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();   
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
