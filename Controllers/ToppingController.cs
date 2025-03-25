
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToppingController(PizzaContext context, ILogger<ToppingController> logger) : ControllerBase // A PromotionsContext is injected into controller via Primary Constructor DI.
{

    PizzaContext _context = context;
    private readonly ILogger _logger = logger;

    /// <summary>
    /// View the Toppings
    /// </summary>
    /// <returns>List of Toppings</returns>
    [HttpGet]
    [Produces("application/json")]
    public IEnumerable<Topping> Get()
    {
        _logger.LogInformation("Fetching Toppings...");

        foreach (var topping in _context.Toppings)
        {
            _logger.LogInformation("INFO: Id: {}, Name: {}, Calories: {}", topping.Id, topping.Name, topping.Calories);
            _logger.LogCritical("CRITICAL: Id: {}, Name: {}, Calories: {}", topping.Id, topping.Name, topping.Calories);
            _logger.LogWarning("WARN: Id: {}, Name: {}, Calories: {}", topping.Id, topping.Name, topping.Calories);
        }
        return [.. _context.Toppings.AsNoTracking()];
    }


}
