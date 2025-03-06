using ContosoPizza.Models;
using ContosoPizza.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace ContosoPizza.Services;

// Pizza to Topping(s): One to many
// Topping to Pizza(s): One to many

// Pizza to Sauce: One to One
// Sauce to Pizzas: One to Many

public class PizzaService
{
    PizzaContext _context;
    public PizzaService(PizzaContext context)
    {
        _context = context;
    }

    public IEnumerable<Pizza> GetAll()
    {
        // The Pizzas collection contains all the rows in the pizzas table.
        // The AsNoTracking extension method instructs EF Core to disable change tracking. Because this operation is read-only, AsNoTracking can optimize performance.
        // All of the pizzas are returned with ToList.
        // Remember to include the toppings & sauce in the response.
        
        return _context.Pizzas
            .Include(p => p.Toppings)
            .Include(p => p.Sauce)
            .AsNoTracking() // faster for readonly purpose
            .ToList();
    }

    public Pizza? GetById(int id)
    {
        /*
        The Include extension method takes a lambda expression to specify that the Toppings and Sauce navigation properties are to be included in the result by using eager loading. Without this expression, EF Core returns null for those properties.
        The SingleOrDefault method returns a pizza that matches the lambda expression.
        If no records match, null is returned.
        If multiple records match, an exception is thrown.
        The lambda expression describes records where the Id property is equal to the id parameter.
        */
        return _context.Pizzas
        .Include(p => p.Toppings)
        .Include(p => p.Sauce)
        .AsNoTracking()
        .SingleOrDefault(p => p.Id == id);

    }

    public async Task<Pizza> Create(Pizza newPizza)
    {
        // newPizza is assumed to be a valid object. EF Core doesn't do data validation, so the ASP.NET Core runtime or user code must handle any validation.
        // The Add method adds the newPizza entity to the EF Core object graph.
        // The SaveChanges method instructs EF Core to persist the object changes to the database.
        _context.Pizzas.Add(newPizza);
        await _context.SaveChangesAsync();

        return newPizza;
    }

    public void AddTopping(int PizzaId, int ToppingId)
    {

        // References to existing Pizza and Topping objects are created by using Find.
        // The Topping object is added to the Pizza.Toppings collection with the Add method. A new collection is created if it doesn't exist.
        // The SaveChanges method instructs EF Core to persist the object changes to the database.

        // get pizza and topping
        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var toppingToAdd = _context.Toppings.Find(ToppingId);
        // handle exceptions
        if (pizzaToUpdate == null || toppingToAdd is null)
        {
            throw new InvalidOperationException("Pizza or topping does not exist");
        }

        // alternative syntax: pizzaToUpdate.Toppings ??= [];
        if (pizzaToUpdate.Toppings == null)
        {
            pizzaToUpdate.Toppings = new List<Topping>();
        }

        // add toppings to pizza
        pizzaToUpdate.Toppings.Add(toppingToAdd);

        // save changes
        _context.SaveChanges();
    }

    public void UpdateSauce(int PizzaId, int SauceId)
    {
        // References to existing Pizza and Sauce objects are created by using Find. Find is an optimized method to query records by their primary key. Find searches the local entity graph first before it queries the database.
        // The Pizza.Sauce property is set to the Sauce object.
        // An Update method call is unnecessary because EF Core detects that you set the Sauce property on Pizza.
        // The SaveChanges method instructs EF Core to persist the object changes to the database.

        var pizzaToUpdate = _context.Pizzas.Find(PizzaId);
        var sauceToUpdate = _context.Sauces.Find(SauceId);

        if(pizzaToUpdate is null || sauceToUpdate is null)
        {
            throw new InvalidOperationException("No existing pizza or sauce found");
        }

        pizzaToUpdate.Sauce = sauceToUpdate;
        _context.SaveChanges();
    }

    public void DeleteById(int id)
    {
        // The Find method retrieves a pizza by the primary key (which is Id in this case).
        // The Remove method removes the pizzaToDelete entity in EF Core's object graph.
        // The SaveChanges method instructs EF Core to persist the object changes to the database.
        Pizza? pizza = _context.Pizzas.Find(id);
        if (pizza != null)
        {
            _context.Pizzas.Remove(pizza);
            _context.SaveChanges();
        }
    }

    public PizzaDTO ItemToDTO(Pizza pizza)
    {
        return new() {
            Id = pizza.Id,
            Name = pizza.Name,
            Toppings = pizza.Toppings,
            Sauce = pizza.Sauce
        };
    }
}