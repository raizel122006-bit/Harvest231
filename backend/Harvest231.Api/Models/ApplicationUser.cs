using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Harvest231.Api.Models;

public class ApplicationUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string FullName { get; set; } = string.Empty;
}