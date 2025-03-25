using System.ComponentModel.DataAnnotations;

namespace ContosoPizza.Models;

public class PizzaCreateDTO
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    public int SauceId { get; set; }
    public ICollection<int> ToppingIds { get; set; } = [];
}