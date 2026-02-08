using System.Net;
using System.Text.Json;
using VueApp1.Server.Models;

namespace VueApp1.Server.Services
{
    public class AddressService
    {
        private readonly HttpClient _httpClient;
        private readonly DapperService _dapperService;
        public AddressService(HttpClient httpClient,
                             DapperService dapperService) {
            _httpClient = httpClient;
            _dapperService = dapperService;
        }
        public async Task<CoordinatesDto> GetCoordinatesFromAddress(AddressDto address)
        {
            var url =
                $"https://nominatim.openstreetmap.org/search" +
                $"?q={Uri.EscapeDataString(address.Address)}" +
                $"&format=json&limit=1";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "VueApp1");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var results = JsonSerializer.Deserialize<CoordinatesDto[]>(content);
            if(results == null || results.Length == 0)
            {
                return new CoordinatesDto
                {
                    lat = null,
                    lon = null
                };
            }

            var result = results[0];
            return new CoordinatesDto
            {
                lat = result.lat,
                lon = result.lon
            };
        }

        public async Task<AddressDto> GetAddressFromCoordinates(CoordinatesDto coords)
        {
            var url =
                $"https://nominatim.openstreetmap.org/reverse" +
                $"?lat={coords.lat}" +
                $"&lon={coords.lon}"+
                $"&format=json" +
                $"&limit=1";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("User-Agent", "VueApp1");

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var address = JsonSerializer.Deserialize<AddressDto>(content);
            return address;
        }
        public async Task<List<MachineDto>> GetMachineList(CoordinatesDto coordinates)
        {
            const double EarthRadiusKm = 6371;

            double lat = double.Parse(coordinates.lat) * Math.PI / 180;
            double lon = double.Parse(coordinates.lon) * Math.PI / 180;

            var data = await _dapperService.GetAllAsync();
            var machineList = data.ToList();
            foreach (var machine in machineList)
            {
                double machineLatRad = machine.lat * Math.PI / 180;
                double machineLonRad = machine.lon * Math.PI / 180;

                double dlat = machineLatRad - lat;
                double dlon = machineLonRad - lon;

                double a = Math.Sin(dlat / 2) * Math.Sin(dlat / 2) +
                           Math.Cos(lat) * Math.Cos(machineLatRad) *
                           Math.Sin(dlon / 2) * Math.Sin(dlon / 2);

                double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
                double distance = EarthRadiusKm * c;
                machine.distance = distance;
            }
            return machineList.ToList();
        }
        public async Task<MachineParamsDto> GetMachineParam(string machineId)
        {
            return await _dapperService.GetAllParamsAsyncByID(machineId);
        }
    }
}
