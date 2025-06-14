using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;

namespace ContosoPizza.Data;

public class PizzaContext : DbContext
{
  // The constructor accepts a parameter of type DbContextOptions<PizzaContext>. The constructor allows external code to pass in the configuration so that the same DbContext can be shared between test and production code, and even be used with different providers.
  public PizzaContext (DbContextOptions<PizzaContext> options) : base(options) 
  {}
  // Enable sensitive data logging
  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.EnableSensitiveDataLogging();

  protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // explicitly define PKs
        // improve logic with BaseConfig.Id; allows clean common config declarations
        modelBuilder.Entity<Pizza>(entity => {
          entity.HasKey(k => k.Id);
        });
        modelBuilder.Entity<Topping>(entity => {
          entity.HasKey(k => k.Id);
        });
        modelBuilder.Entity<Sauce>(entity => {
          entity.HasKey(k => k.Id);
        });
        
        modelBuilder.Entity<Topping>()
          .Property(t => t.Calories)
          .HasPrecision(4, 1); // 4 total digits, 1 total digt after decimal
    }

  // The DbSet<T> properties correspond to tables to create in the database.
  // The table names match the DbSet<T> property names in the PizzaContext class. You can override this behavior if needed.
  // When instantiated, PizzaContext exposes the Pizzas, Toppings, and Sauces properties. Changes you make to the collections that those properties expose are propagated to the database.
  public DbSet<Pizza> Pizzas => Set<Pizza>();
  public DbSet<Topping> Toppings => Set<Topping>();
  public DbSet<Sauce> Sauces => Set<Sauce>();
}
