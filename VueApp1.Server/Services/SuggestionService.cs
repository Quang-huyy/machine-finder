using VueApp1.Server.Common.Errors;
using VueApp1.Server.Models;
using VueApp1.Server.Domain.Enums;
using VueApp1.Server.Domain.Mappings;

namespace VueApp1.Server.Services
{
    public class SuggestionService
    {
        private readonly DapperService _dapperService;
        public SuggestionService(
            DapperService dapperService)
        {
            _dapperService = dapperService;
        }
        public async Task<List<ServiceError>> submitMachineSuggestion(MachineSuggestionDto[] suggestions)
        {
            var errors = new List<ServiceError>();
            foreach (var suggestion in suggestions)
            {
                var properties = typeof(MachineSuggestionDto).GetProperties();
                foreach (var prop in properties)
                {
                    var value = prop.GetValue(suggestion)?.ToString();
                    if (string.IsNullOrEmpty(value))
                    {
                        errors.Add(new ServiceError
                        {
                            Code = Errors.EMPTY_FIELD.ToString(),
                            Message = $"The field {prop.Name} cannot be empty.",
                            Field = prop.Name,
                            MachineId = suggestion.MachineID.ToString(),
                        });
                    }
                }
            }
            if (errors.Count > 0)
            {
                return errors;
            }
            else
            {
                var sqlUpdateResult = await _dapperService.SaveSuggestions(suggestions);
                if (!sqlUpdateResult)
                {
                    errors.Add(new ServiceError
                    {
                        Code = Errors.SQL_UPDATE_FAILED.ToString(),
                        Message = "Failed to update the database with the provided suggestions.",
                        Field = "Database",
                        MachineId = string.Join(", ", suggestions.Select(s => s.MachineID))
                    });
                }
                else
                {
                    return errors;
                }
            }
            return errors;
        }
    }
}
