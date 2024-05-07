using System.ComponentModel.DataAnnotations;

namespace Kolokwium_1.Models.DTOs;

public class NewBookDto
{
    [Required]
    [MaxLength(100)]
    public string title { get; set; }
    public List<NewGenresDto> genres { get; set; }
}

public class NewGenresDto
{
    [Required]
    public int PK { get; set; }
}