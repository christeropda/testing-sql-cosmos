
using Azure.Identity;
using cosmosdb_test.Interfaces;
using cosmosdb_test.Models.Context;
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

        // Add to config from key-vault.
        builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration["KeyVault:Name"]!), new DefaultAzureCredential());
        
        // Add services to the container.
        builder.Services.AddControllers();

        // Add database context
        builder.Services.AddDbContext<GameContextCosmos>(options => options.UseCosmos(builder.Configuration["COSMOSENDPOINT"]!, builder.Configuration["COSMOSKEY"]!, databaseName: "GameDB"));
        builder.Services.AddDbContext<GameContextSql>(options => options.UseSqlServer(builder.Configuration["SQLCONNECTIONSTRING"]!));

        // Add services
        builder.Services.AddMapster();
        builder.Services.AddOpenApi();
        builder.Services.AddTransient<IGameState, GameStateService>();
        builder.Services.AddScoped<ICosmosPlayedGameService, CosmosPlayedGameService>();
        builder.Services.AddScoped<ISqlPlayedGameService, SqlPlayedGameService>();

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
