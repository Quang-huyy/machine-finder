using Dapper;
using Microsoft.Data.SqlClient;
using VueApp1.Server.Models;

public class DapperService
{
    private readonly string _connectionString;

    public DapperService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        // Dynamically select connection string based on environment
        string connectionStringKey = environment.IsProduction() ? "prod_DefaultConnection" : "dev_DefaultConnection";
        
        _connectionString = configuration.GetConnectionString(connectionStringKey)
            ?? throw new InvalidOperationException(
                $"Connection string '{connectionStringKey}' is not configured. " +
                $"Environment: {environment.EnvironmentName}"
            );
    }

    public async Task<IEnumerable<MachineDto>> GetAllAsync()
    { 
        using var connection = new SqlConnection(_connectionString);
        var sql = $"SELECT * FROM Machine";
        return await connection.QueryAsync<MachineDto>(sql, commandTimeout: 60);
    }

    public async Task<MachineParamsDto> GetAllParamsAsyncByID(string machineId)
    {
        using var connection = new SqlConnection(_connectionString);
        var sql = $"SELECT TagKey, TagValue FROM MachineParams WHERE MachineID = @MachineID";
        var results = await connection.QueryAsync<dynamic>(sql, new { MachineID = machineId }, commandTimeout: 60);
        var dto = new MachineParamsDto();

        foreach (var row in results)
        {
            string key = row.TagKey;
            string value = row.TagValue;
            
            switch (key.ToLower()) // Using ToLower() makes it safer against DB typos
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
