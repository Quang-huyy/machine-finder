using MachineFinder.Server.Common.Errors;
using MachineFinder.Server.Domain.Enums;
using MachineFinder.Server.Models;
using MachineFinder.Server.Repositories;

namespace MachineFinder.Server.Services
{
    public class SuggestionService : ISuggestionService
    {
        private readonly IMachineRepository _machineRepository;
        public SuggestionService(
            IMachineRepository machineRepository)
        {
            _machineRepository = machineRepository;
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
                var sqlUpdateResult = await _machineRepository.SaveSuggestions(suggestions);
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
