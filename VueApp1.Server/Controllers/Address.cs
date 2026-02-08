using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Models;
using VueApp1.Server.Services;
namespace VueApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AddressController : ControllerBase
    {
        private readonly AddressService _addressService;
        public AddressController(
            AddressService addressService)
        {
            _addressService = addressService;
        }


        [HttpPost("addressToCoordinates")]
        public async Task<IActionResult> AddressToCoordinates([FromBody] AddressDto request)
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

        [HttpPost("coordinatesToAddress")]
        public async Task<IActionResult> CoordinatesToAddress([FromBody] CoordinatesDto coords)
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

        [HttpPost("getResults")]
        public async Task<IActionResult> getResults([FromBody] CoordinatesDto coords, [FromQuery] int maxDistance)
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
                var paramsInfo = await _addressService.GetMachineParam(machine.id.ToString());
                machine.params_info = paramsInfo;
            }
            return Ok(nearbyMachine);
        }
    }
}
