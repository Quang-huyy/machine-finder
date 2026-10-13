using MachineFinder.Server.Common.Errors;
using MachineFinder.Server.Models;

namespace MachineFinder.Server.Services
{
    public interface ISuggestionService
    {
        Task<List<ServiceError>> submitMachineSuggestion(MachineSuggestionDto[] suggestions);
    }
}
