using MachineFinder.Server.Models;

namespace MachineFinder.Server.Services
{
    public interface IAddressService
    {
        Task<CoordinatesDto> GetCoordinatesFromAddress(AddressDto address);
        Task<AddressDto> GetAddressFromCoordinates(CoordinatesDto coords);
        Task<List<MachineDto>> GetMachineList(CoordinatesDto coordinates);
        Task<MachineParamsDto> GetMachineParam(long machineId);
    }
}
