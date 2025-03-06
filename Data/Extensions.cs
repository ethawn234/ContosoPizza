namespace ContosoPizza.Data;

// The DbInitializer class is ready to seed the database, but it needs to be called from Program.cs.
// The following is an extension method for IHost that calls DbInitializer.Initialize:
public static class Extensions
{
  public static void CreateDbIfNotExists(this IHost host) // The CreateDbIfNotExists method is defined as an extension of IHost.
  {
    {
      using (var scope = host.Services.CreateScope())
      {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<PizzaContext>(); // A reference to the PizzaContext service is created.
        context.Database.EnsureCreated(); // EnsureCreated ensures that the database exists. If a database doesn't exist, EnsureCreated creates a new database. The new database isn't configured for migrations, so use this method with caution.

        DbInitializer.Initialize(context); // The DbIntializer.Initialize method is called. The PizzaContext object is passed as a parameter.
      }
    }
  }
}