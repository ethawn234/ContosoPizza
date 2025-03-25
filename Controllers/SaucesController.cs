
using ContosoPizza.Models;
using Microsoft.AspNetCore.Mvc;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SaucesController(PizzaContext context) : ControllerBase
{
    PizzaContext _context = context;

    [HttpGet]
    [Produces("application/json")]
    public IEnumerable<Sauce> Get()
    {
        return [.. _context.Sauces.AsNoTracking()];
    }
}