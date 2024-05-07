using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Kolokwium_1.Models.DTOs;

public class BooksDto
{
    [Required]
    public int id { get; set; }
    [Required]
    [MaxLength(100)]
    public string title { get; set; }
    public List<GenresDto> genres { get; set; }
}

public class GenresDto
{
    [Required]
    [MaxLength(100)]
    public string nam { get; set; }
}