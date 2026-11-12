using Harvest231.Api.DTOs;
using Harvest231.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Harvest231.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService){
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto){
        var result = await _authService.RegisterAsync(dto);

        if(!result.Success){
            return BadRequest(result);
        }

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto){
        var result = await _authService.LoginAsync(dto);

        if(!result.Success){
            return Unauthorized(result);
        }

        return Ok(result);
    }
}