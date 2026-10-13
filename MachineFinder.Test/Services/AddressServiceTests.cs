namespace MachineFinder.Test;

using Xunit;
using Moq;
using MachineFinder.Server.Services;
using MachineFinder.Server.Repositories;
using MachineFinder.Server.Models;

public class AddressServiceTests
{
    private readonly Mock<IMachineRepository> _mockMachineRepository;
    private readonly Mock<HttpClient> _mockHttpClient;
    private readonly AddressService _addressService;

    public AddressServiceTests()
    {
        _mockMachineRepository = new Mock<IMachineRepository>();
        _mockHttpClient = new Mock<HttpClient>();
        _addressService = new AddressService(_mockHttpClient.Object, _mockMachineRepository.Object);
    }

    [Fact]
    public async Task GetMachineParam_WithValidMachineId_ShouldReturnMachineParamsDto()
    {
        // Arrange
        long machineId = 778863710;
        var expectedDto = new MachineParamsDto
        {
            brand = "TestBrand",
            charge = "50",
            cash = "100"
        };

        _mockMachineRepository
            .Setup(repo => repo.GetAllParamsAsyncByID(machineId))
            .ReturnsAsync(expectedDto);

        // Act
        var result = await _addressService.GetMachineParam(machineId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedDto.brand, result.brand);
        Assert.Equal(expectedDto.charge, result.charge);
        Assert.Equal(expectedDto.cash, result.cash);
        _mockMachineRepository.Verify(repo => repo.GetAllParamsAsyncByID(machineId), Times.Once);
    }

    [Fact]
    public async Task GetMachineList_ShouldReturnListOfMachines()
    {
        // Arrange
        var coords = new CoordinatesDto { lat = "45.73", lon = "4.88" };
        var mockMachines = new List<MachineDto>
        {
            new MachineDto { MachineID = 1, position = 0, lat = 45.73f, lon = 4.88f },
            new MachineDto { MachineID = 2, position = 1, lat = 45.74f, lon = 4.89f }
        };

        _mockMachineRepository
            .Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(mockMachines);

        // Act
        var result = await _addressService.GetMachineList(coords);

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        _mockMachineRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetMachineParam_RepositoryThrowsException_ShouldThrowException()
    {
        // Arrange
        long machineId = 999999999;
        _mockMachineRepository
            .Setup(repo => repo.GetAllParamsAsyncByID(machineId))
            .ThrowsAsync(new Exception("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(
            () => _addressService.GetMachineParam(machineId)
        );
    }
}
