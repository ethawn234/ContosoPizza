using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace ContosoPizza.Models;

public class PizzaCreateBody
{
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string? Name { get; set; }
    public int SauceId { get; set; }
    public ICollection<int> ToppingIds { get; set; } = [];
}