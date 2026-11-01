using System.ComponentModel.DataAnnotations;

namespace Harvest231.Api.DTOs;

public class UpdateFarmDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public double SizeHectares { get; set; }
}