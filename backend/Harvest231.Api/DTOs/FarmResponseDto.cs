namespace Harvest231.Api.DTOs;

public class FarmResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public double SizeHectares { get; set; }
}