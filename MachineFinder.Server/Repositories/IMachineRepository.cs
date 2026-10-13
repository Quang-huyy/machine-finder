using MachineFinder.Server.Models;

namespace MachineFinder.Server.Repositories
{
    public interface IMachineRepository
    {
        Task<IEnumerable<MachineDto>> GetAllAsync();
        Task<MachineParamsDto> GetAllParamsAsyncByID(long machineId);
        Task<bool> SaveSuggestions(MachineSuggestionDto[] suggestions);
    }
}
