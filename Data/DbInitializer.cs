using ContosoPizza.Models;
using Microsoft.EntityFrameworkCore;

namespace ContosoPizza.Data
{
  // This database seeding code doesn't account for race conditions, so be careful when using it in a distributed environment without mitigating changes.
  public static class DbInitializer
  {
    // The DbInitializer class and Initialize method are both defined as static.
    public static async Task Initialize(PizzaContext context) // Initialize accepts a PizzaContext object as a parameter.
    {
      var maxId = await context.Pizzas.MaxAsync(p => (int?)p.Id);
      // If there are no records in any of the three tables, Pizza, Sauce, and Topping objects are created.
      if (maxId == null)
      {
        // create Toppings
        var pepperoniTopping = new Topping { Name = "Pepperoni", Calories = 130 };
        var sausageTopping = new Topping { Name = "Sausage", Calories = 100 };
        var hamTopping = new Topping { Name = "Ham", Calories = 70 };
        var chickenTopping = new Topping { Name = "Chicken", Calories = 50 };
        var pineappleTopping = new Topping { Name = "Pineapple", Calories = 75 };

        // create Sauces
        var tomatoSauce = new Sauce { Name = "Tomato", IsVegan = true };
        var pestoSauce = new Sauce { Name = "Pesto", IsVegan = false };

        // create Pizzas
        var pizzas = new Pizza[]{
            new() {
              Name = "Pepperoni Delight",
              Sauce = tomatoSauce,
              Toppings = [pepperoniTopping]
            },
            new Pizza {
              Name = "Meat Lovers",
              Sauce = tomatoSauce,
              Toppings = [pepperoniTopping, sausageTopping, hamTopping, chickenTopping]
            },
            new Pizza {
              Name = "Pineapple Paradise",
              Sauce = pestoSauce,
              Toppings = new List<Topping>
                {
                  pineappleTopping
                }
            }
          };

        // context.Pizzas.AddRange(pizzas); // The Pizza objects (and their Sauce and Topping navigation properties) are added to the object graph by using AddRange.
        // context.SaveChanges(); // The object graph changes are committed to the database by using SaveChanges.
        await context.Pizzas.AddRangeAsync(pizzas);
        await context.SaveChangesAsync();
        maxId = await context.Pizzas.MaxAsync(p => (int?)p.Id); // refresh maxId after inserting
      }
      // reseed
      await context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('Pizzas', RESEED, {maxId ?? 0})");
      }
  }
}