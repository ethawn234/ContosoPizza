using ContosoPizza.Services;
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("[controller]")]
public class ContosoPizzaController : ControllerBase
{
    PizzaService _service;
    private readonly ILogger _pizzaLogger;

    public ContosoPizzaController(PizzaService service, ILogger<ContosoPizzaController> logger)
    {
        _service = service;
        _pizzaLogger = logger;
    }

    /// <summary>
    /// Admin Fetch returns all fields including secret fields
    /// </summary>
    /// <returns>A list of pizzas with their secrets</returns>
    [HttpGet("admin")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public IEnumerable<Pizza> GetAdmin()
    {
        return _service.GetAll();
    }

    /// <summary>
    /// Fetches all pizzas
    /// </summary>
    /// <returns>A List of Pizzas</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public IEnumerable<PizzaDTO> GetAll()
    {
        var pizzas = _service.GetAll();
        _pizzaLogger.LogWarning("Fetching all Pizzas: {}.", pizzas); // Fetching all Pizzas: ContosoPizza.Models.Pizza, ContosoPizza.Models.Pizza, ContosoPizza.Models.Pizza, ContosoPizza.Models.Pizza.

        foreach (var pizza in pizzas)
        {
            _pizzaLogger.LogWarning("{}: {}.", pizza.Name, pizza.Toppings); // Only Name: (null)...
        }

        return pizzas.Select(_service.ItemToDTO);
    }

    /// <summary>
    /// Get a Pizza by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>A Pizza or NotFound</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Produces("application/json")]
    public ActionResult<PizzaDTO> GetById(int id)
    {
        var pizza = _service.GetById(id);

        if (pizza is not null)
        {
            return _service.ItemToDTO(pizza);
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Create a new Pizza
    /// </summary>
    /// <param name="pizzaCreateBody"></param>
    /// <returns>The Pizza</returns>
    /// <remarks>
    /// Sample request:
    ///     <code>
    ///     POST /ContosoPizza
    ///         {
    ///             "id": 0,
    ///             "name": "Pepperoni Stuffed Crust"
    ///         }
    ///     </code>   
    ///  </remarks>
    ///  
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Produces("application/json")]
    public async Task<IActionResult> Create(PizzaCreateBody pizzaCreateBody)
    {
        var pizza = await _service.Create(pizzaCreateBody);

        return CreatedAtAction(nameof(GetById), new { id = pizza!.Id }, pizza);
    }

    /// <summary>
    /// Select a topping and add it to your pizza!
    /// </summary>
    /// <param name="pizzaId"></param>
    /// <param name="toppingId"></param>
    /// <returns></returns>
    [HttpPut("{pizzaId}/addtopping")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AddTopping(int pizzaId, int toppingId)
    {
        var pizzaToUpdate = _service.GetById(pizzaId);

        if (pizzaToUpdate is not null)
        {
            _service.AddTopping(pizzaId, toppingId);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Select a sauce for your pizza
    /// </summary>
    /// <param name="id"></param>
    /// <param name="sauceId"></param>
    /// <returns></returns>
    [HttpPut("{id}/updatesauce")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateSauce(int id, int sauceId)
    {
        var pizzaToUpdate = _service.GetById(id);

        if (pizzaToUpdate is not null)
        {
            _service.UpdateSauce(id, sauceId);
            return NoContent();
        }
        else
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Delete a pizza by Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(int id)
    {
        var pizza = _service.GetById(id);

        if (pizza is not null)
        {
            _service.DeleteById(id);
            return Ok();
        }
        else
        {
            return NotFound();
        }
    }


}