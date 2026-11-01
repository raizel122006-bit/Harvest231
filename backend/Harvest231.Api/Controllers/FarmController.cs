using Harvest231.Api.DTOs;
using Harvest231.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Harvest231.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FarmsController : ControllerBase
{
    private readonly IFarmService _farmService;

    public FarmsController(IFarmService farmService)
    {
        _farmService = farmService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllFarms()
    {
        var farms = await _farmService.GetAllFarmsAsync();

        return Ok(farms);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetFarmById(int id)
    {
        var farm = await _farmService.GetFarmByIdAsync(id);

        if (farm == null)
        {
            return NotFound();
        }

        return Ok(farm);
    }

    [HttpPost]
    public async Task<IActionResult> CreateFarm(CreateFarmDto dto)
    {
        var farm = await _farmService.CreateFarmAsync(dto);

        return CreatedAtAction(
            nameof(GetFarmById),
            new { id = farm.Id },
            farm
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFarm(int id, UpdateFarmDto dto)
    {
        var updated = await _farmService.UpdateFarmAsync(id, dto);

        if (!updated)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFarm(int id)
    {
        var deleted = await _farmService.DeleteFarmAsync(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }
}