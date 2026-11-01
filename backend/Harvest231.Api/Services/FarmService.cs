using Harvest231.Api.Data;
using Harvest231.Api.DTOs;
using Harvest231.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Harvest231.Api.Services;

public class FarmService : IFarmService
{
    private readonly AppDbContext _context;

    public FarmService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<FarmResponseDto>> GetAllFarmsAsync()
    {
        return await _context.Farms
            .AsNoTracking()
            .Select(farm => new FarmResponseDto
            {
                Id = farm.Id,
                Name = farm.Name,
                Location = farm.Location,
                SizeHectares = farm.SizeHectares
            })
            .ToListAsync();
    }

    public async Task<FarmResponseDto?> GetFarmByIdAsync(int id)
    {
        return await _context.Farms
            .AsNoTracking()
            .Where(farm => farm.Id == id)
            .Select(farm => new FarmResponseDto
            {
                Id = farm.Id,
                Name = farm.Name,
                Location = farm.Location,
                SizeHectares = farm.SizeHectares
            })
            .FirstOrDefaultAsync();
    }

    public async Task<FarmResponseDto> CreateFarmAsync(CreateFarmDto dto)
    {
        var farm = new Farm
        {
            Name = dto.Name,
            Location = dto.Location,
            SizeHectares = dto.SizeHectares
        };

        _context.Farms.Add(farm);

        await _context.SaveChangesAsync();

        return new FarmResponseDto
        {
            Id = farm.Id,
            Name = farm.Name,
            Location = farm.Location,
            SizeHectares = farm.SizeHectares
        };
    }

    public async Task<bool> UpdateFarmAsync(int id, UpdateFarmDto dto)
    {
        var farm = await _context.Farms.FindAsync(id);

        if (farm == null)
        {
            return false;
        }

        farm.Name = dto.Name;
        farm.Location = dto.Location;
        farm.SizeHectares = dto.SizeHectares;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteFarmAsync(int id)
    {
        var farm = await _context.Farms.FindAsync(id);

        if (farm == null)
        {
            return false;
        }

        _context.Farms.Remove(farm);

        await _context.SaveChangesAsync();

        return true;
    }
}