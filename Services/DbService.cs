using Dapper;
using Npgsql;
using System.Data;
using System.Linq;

using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;

using Microsoft.Extensions.DependencyInjection;

public class DbService : IDbService
{
    /// <summary>
    /// Represents an open connection to a database.
    /// </summary>
    private readonly IDbConnection _db;

    public DbService(IConfiguration configuration)
    {
        _db = new NpgsqlConnection(
            configuration.GetSection(configuration.GetConnectionString("DatabaseName")!).Value
        );

        var serviceProvider = CreateServices(configuration);
        var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
        runner.MigrateUp();
    }

    async Task<T> IDbService.GetAsync<T>(string command, object parms)
    {
        T? result;

        result = (await _db.QueryAsync<T>(command, parms).ConfigureAwait(false)).FirstOrDefault();

        return result!;
    }

    public async Task<T?> GetAsync<T>(string command, object parms)
        where T : class
    {
        return await ((IDbService)this).GetAsync<T>(command, parms);
    }

    public async Task<List<T>> GetAll<T>(string command, object parms)
    {
        List<T> result = new();

        result = (await _db.QueryAsync<T>(command, parms)).ToList();

        return result;
    }

    public async Task<int> EditData(string command, object parms)
    {
        int result;

        result = await _db.ExecuteAsync(command, parms);

        return result;
    }

    private static IServiceProvider CreateServices(IConfiguration configuration)
    {
        return new ServiceCollection()
            .AddFluentMigratorCore()
            .ConfigureRunner(
                rb =>
                    rb.AddPostgres()
                        .WithGlobalConnectionString(
                            configuration
                                .GetSection(configuration.GetConnectionString("DatabaseName")!)
                                .Value
                        )
                        .ScanIn(typeof(DbService).Assembly)
                        .For.Migrations()
            )
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            .BuildServiceProvider(false);
    }
}
