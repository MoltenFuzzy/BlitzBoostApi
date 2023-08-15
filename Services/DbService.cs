using Dapper;
using Npgsql;
using System.Data;

public class DbService : IDbService
{
    /// <summary>
    /// Represents an open connection to a database.
    /// </summary>
    private readonly IDbConnection _db;

    public DbService(IConfiguration configuration)
    {
        _db = new NpgsqlConnection(configuration.GetConnectionString("DatabaseName"));
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
}
