using Harvest231.Api.DTOs;

namespace Harvest231.Api.Services;

public interface IFarmService
{
    Task<List<FarmResponseDto>> GetAllFarmsAsync();

    Task<FarmResponseDto?> GetFarmByIdAsync(int id);

    Task<FarmResponseDto> CreateFarmAsync(CreateFarmDto dto);

    Task<bool> UpdateFarmAsync(int id, UpdateFarmDto dto);

    Task<bool> DeleteFarmAsync(int id);
}