namespace ContosoPizza.Data;

// The DbInitializer class is ready to seed the database, but it needs to be called from Program.cs.
// The following is an extension method for IHost that calls DbInitializer.Initialize:
public static class Extensions
{
  public static async Task CreateDbIfNotExists(this IHost host) // The CreateDbIfNotExists method is defined as an extension of IHost.
  {
    {
      using var scope = host.Services.CreateScope();
      var services = scope.ServiceProvider;
      var context = services.GetRequiredService<PizzaContext>(); // A reference to the PizzaContext service is created.
      await DbInitializer.Initialize(context); // The DbIntializer.Initialize method is called. The PizzaContext object is passed as a parameter.
    }
  }
}