using System.ComponentModel.DataAnnotations;

namespace Harvest231.Api.Models;

public class Farm{

    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string Location { get; set; } = string.Empty;

    [Range(0.01, 100000)]
    public double SizeHectares { get; set; }

    [Required]
    public string OwnerId { get; set; } = string.Empty;

    public ApplicationUser Owner { get; set; } = null!;
}