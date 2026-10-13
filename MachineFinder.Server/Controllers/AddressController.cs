using Microsoft.AspNetCore.Mvc;
using MachineFinder.Server.Models;
using MachineFinder.Server.Services;
namespace MachineFinder.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;
        public AddressController(
            IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("addressToCoordinates")]
        public async Task<IActionResult> AddressToCoordinates([FromQuery] AddressDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Address))
                return BadRequest("Address is required");

            var result = await _addressService.GetCoordinatesFromAddress(request);
            if(result == null)
            {
                return NotFound("address not found");
            }
            return Ok(new
            {
                lat = double.Parse(result.lat),
                lon = double.Parse(result.lon)
            });
        }

        [HttpGet("coordinatesToAddress")]
        public async Task<IActionResult> CoordinatesToAddress([FromQuery] CoordinatesDto coords)
        {
            if(string.IsNullOrWhiteSpace(coords.lat) || string.IsNullOrWhiteSpace(coords.lon)){
                return BadRequest("Invalid lat/lon");
            }
            var result = await _addressService.GetAddressFromCoordinates(coords);
            return Ok(new
            {
                address = result.Address
            });
        }

        [HttpGet("getResults")]
        public async Task<IActionResult> getResults([FromQuery] CoordinatesDto coords, [FromQuery] int maxDistance)
        {
            if (string.IsNullOrWhiteSpace(coords.lat) || string.IsNullOrWhiteSpace(coords.lon))
            {
                return BadRequest("lat/lon is empty");
            }
            var MachineListResult = await _addressService.GetMachineList(coords);
            var nearbyMachine = MachineListResult.Where(m => m.distance <= maxDistance)
            .OrderBy(m => m.distance).ToList();
            var position = 1;
            foreach(var machine in nearbyMachine)
            {
                machine.position = position;
                position++;
                var paramsInfo = await _addressService.GetMachineParam(machine.MachineID);
                machine.params_info = paramsInfo;
            }
            return Ok(nearbyMachine);
        }
    }
}
