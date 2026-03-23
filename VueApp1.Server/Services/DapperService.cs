using Dapper;
using Microsoft.Data.SqlClient;
using Npgsql;
using System.Data;
using VueApp1.Server.Models;

public class DapperService
{
    private readonly string _connectionString;

    public DapperService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _connectionString = configuration["SupabaseDb"];
    }
    public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);
        
    public async Task<IEnumerable<MachineDto>> GetAllAsync()
    {
        using var connection = CreateConnection();
        var sql = "SELECT * FROM \"Machine\"";
        return await connection.QueryAsync<MachineDto>(sql);
    }

    public async Task<MachineParamsDto> GetAllParamsAsyncByID(long machineId)
    {
        using var connection = CreateConnection();
        var sql = $"SELECT * FROM \"MachineParams\" WHERE \"MachineID\" = @MachineID";
        var results = await connection.QueryAsync<dynamic>(sql, new { MachineID = machineId });
        var dto = new MachineParamsDto();

        foreach (var row in results)
        {
            string key = row.TagKey;
            string value = row.TagValue;
            
            switch (key.ToLower())
            {
                case "brand": dto.brand = value; break;
                case "charge": dto.charge = value; break;
                case "check_date": dto.check_date = value; break;
                case "disused": dto.disused = value; break;
                case "fee": dto.fee = value; break;
                case "indoor": dto.indoor = value; break;
                case "opening_hours": dto.opening_hours = value; break;
                case "machine_operator": dto.machine_operator = value; break;
                case "cash": dto.cash = value; break;
                case "coins": dto.coins = value; break;
                case "credit_cards": dto.credit_cards = value; break;
            }
        }
        return dto;
    }

    public async Task<bool> SaveSuggestions(MachineSuggestionDto[] suggestions)
    {
        using var connection = new SqlConnection(_connectionString);
        var rowToUpdate = suggestions.Length;
        var sql = @"INSERT INTO SuggestionParams (MachineID, TagKey, TagValue, SuggestTagValue) VALUES (@MachineID, @TagKey, @TagValue, @SuggestTagValue)";
        var result = await connection.ExecuteAsync(sql, suggestions, commandTimeout: 60);

        return result < rowToUpdate ? false : true;
    }
}
