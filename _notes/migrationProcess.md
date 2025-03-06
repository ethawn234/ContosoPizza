
# Set up a Migration
A migration provides a way to incrementally update the database schema.

### Add Required Packages
`dotnet add package Microsoft.EntityFrameworkCore.Sqlite` *contains the EF Core SQLite database provider and all its dependencies, including the common EF Core services.*

`dotnet add package Microsoft.EntityFrameworkCore.Design` *required for the EF Core tools.*

`dotnet tool install --global dotnet-ef` *This command installs dotnet ef, the tool you use to create migrations and scaffolding. If dotnet ef is already installed, you can update it by running dotnet tool update --global dotnet-ef.*

# Scaffold models and DbContext

Add and configure a DbContext implementation. DbContext is a gateway through which you can interact with the database.

```c#
using Microsoft.EntityFrameworkCore;
using ContosoPizza.Models;

namespace ContosoPizza.Data;

public class PizzaContext : DbContext
{
    public PizzaContext (DbContextOptions<PizzaContext> options)
        : base(options)
    {
    }

    public DbSet<Pizza> Pizzas => Set<Pizza>();
    public DbSet<Topping> Toppings => Set<Topping>();
    public DbSet<Sauce> Sauces => Set<Sauce>();
}
```
In the preceding code:
- The constructor accepts a parameter of type DbContextOptions<PizzaContext>. The constructor allows external code to pass in the configuration so that the same DbContext can be shared between test and production code, and even be used with different providers.
- The DbSet<T> properties correspond to tables to create in the database.
- The table names match the DbSet<T> property names in the PizzaContext class. You can override this behavior if needed.

When instantiated, PizzaContext exposes the Pizzas, Toppings, and Sauces properties. Changes you make to the collections that those properties expose are propagated to the database.

In Program.cs, replace // Add the PizzaContext with the following code:
`builder.Services.AddSqlite<PizzaContext>("Data Source=ContosoPizza.db");`

The preceding code:
  - Registers PizzaContext with the ASP.NET Core dependency injection system.
  - Specifies that PizzaContext uses the SQLite database provider.
Defines a SQLite connection string that points to a local file, ContosoPizza.db.

# Create and run a migration
### Run the following command to generate a migration for creating the database tables:

`dotnet ef migrations add InitialCreate --context PizzaContext`

In the preceding command:
- The migration is named: InitialCreate.
- The --context option specifies the name of the class in the ContosoPizza project, which derives from DbContext.

### Run the following command to apply the InitialCreate migration:

`dotnet ef database update --context PizzaContext`

This command applies the migration. ContosoPizza.db doesn't exist, so this command creates the migration in the project directory.

# Inspect the database

1. On the Explorer pane, right-click the ContosoPizza.db file and select Open Database.


2. Select the SQLite Explorer folder to expand the node and all its child nodes. Right-click ContosoPizza.db and select Show Table `sqlite_master` to view the full database schema and constraints that the migration created.

    - Tables that correspond to each entity were created.
    - Table names were taken from the names of the DbSet properties on the PizzaContext.
    - Properties named Id were inferred to be autoincrementing primary key fields.
    The EF Core primary key and foreign key constraint naming conventions are PK_<primary key property> and FK_<dependent entity>_<principal entity>_<foreign key property>, respectively. The <dependent entity> and <principal entity> placeholders correspond to the entity class names.

# Change the model and update the database schema
1. Make any Model change.
2. Save all your changes and run `dotnet build`.
3. Run the following command to generate a migration for creating the database tables: `dotnet ef migrations add ModelRevisions --context PizzaContext`. This command creates a migration named: ModelRevisions.
    
*To start with a fresh database, stop the app and delete the ContosoPizza.db, .db-shm, and .db-wal files. Then, run the app again.*
    
    - You'll see this message: `An operation was scaffolded that may result in the loss of data. Please review the migration for accuracy`. 
    - This message appears when model relationships change (eg from one-to-one -> one-to-many). 
    - Check the generated migration when this warning appears to make sure that the migration doesn't delete or truncate any data.
4. Run the following command to apply the ModelRevisions migration:
`dotnet ef database update --context PizzaContext`
5. On the title bar of the SQLite Explorer folder, select the Refresh Databases button.
6. In the SQLite Explorer folder, right-click ContosoPizza.db. Select Show Table 'sqlite_master' to view the full database schema and constraints.

    - A join table is created to represent the many-to-many relationship.
    - New fields were added to Toppings and Sauces.
    - Calories is defined as a text column because SQLite doesn't have a matching decimal type.
    - Similarly, IsVegan is defined as an integer column. SQLite doesn't define a bool type.
    - In both cases, EF Core manages the translation.
    The Name column in each table was marked not null, but SQLite doesn't have a MaxLength constraint.

7. In the SQLite Explorer folder, right-click the _EFMigrationsHistory table and select Show Table. The table contains a list of all migrations that are applied to the database. Because you ran two migrations, there are two entries: one for the InitialCreate migration and another for ModelRevisions.

---

Note: This exercise used mapping attributes (data annotations) to map models to the database. As an alternative to mapping attributes, you can use the ModelBuilder fluent API to configure models. Both approaches are valid, but some developers prefer one approach over the other.