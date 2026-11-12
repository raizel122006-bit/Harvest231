
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using Harvest231.Api.DTOs;
using Harvest231.Api.Models;

namespace Harvest231.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        // 1. Validate the requested role
        string role = dto.Role.Trim();

        if (role.Equals("Farmer", StringComparison.OrdinalIgnoreCase))
        {
            role = "Farmer";
        }
        else if (role.Equals("Buyer", StringComparison.OrdinalIgnoreCase))
        {
            role = "Buyer";
        }
        else
        {
            return new AuthResponseDto
            {
                Message = "Invalid role. You can only register as a Farmer or Buyer.",
                Success = false
            };
        }

        // 2. Check whether the email already exists
        var existingUser = await _userManager.FindByEmailAsync(dto.Email);

        if (existingUser != null)
        {
            return new AuthResponseDto
            {
                Message = "An account with this email already exists.",
                Success = false
            };
        }

        // 3. Make sure the role exists
        if (!await _roleManager.RoleExistsAsync(role))
        {
            var roleCreationResult = await _roleManager.CreateAsync(
                new IdentityRole(role));

            if (!roleCreationResult.Succeeded)
            {
                return new AuthResponseDto
                {
                    Message = "Failed to create the requested role.",
                    Success = false
                };
            }
        }

        // 4. Create the user
        var user = new ApplicationUser
        {
            FullName = dto.FullName,
            Email = dto.Email,
            UserName = dto.Email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return new AuthResponseDto
            {
                Message = string.Join(", ", result.Errors.Select(e => e.Description)),
                Success = false
            };
        }

        // 5. Assign the user's role
        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
        {
            await _userManager.DeleteAsync(user);

            return new AuthResponseDto
            {
                Message = "Account creation failed while assigning the role.",
                Success = false
            };
        }

        // 6. Return success response
        return new AuthResponseDto
        {
            Message = "Account registered successfully.",
            Success = true
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        // Find the user by email
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        // Verify password
        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);

        if (!isPasswordValid)
        {
            return new AuthResponseDto
            {
                Success = false,
                Message = "Invalid email or password."
            };
        }

        // Generate JWT token
        var token = await GenerateJwtToken(user);

        return new AuthResponseDto
        {
            Success = true,
            Message = "Login successful.",
            Token = token
        };
    }

    private async Task<string> GenerateJwtToken(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email!),
            new Claim(ClaimTypes.Role, userRoles.First())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
