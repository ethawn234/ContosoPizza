# Reverse-engineer from an existing database

Reverse-engineering is the concept that the database is the source of truth for our Models. 

This means the Model schema is defined by the database, in contrast to the Migrations paradigm where the Model serves as the source of truth.

# Inspect the promotions database
On the Explorer pane, expand the Promotions directory, right-click the Promotions.db file, and then select Open Database.

# Scaffold the promotions context and coupon model

Now, you use the database to scaffold the code:

`dotnet ef dbcontext scaffold "Data Source=Promotions/Promotions.db" Microsoft.EntityFrameworkCore.Sqlite --context-dir Data --output-dir Models`

The preceding command:
- Scaffolds DbContext and model classes by using the provided connection string.
- Specifies to use the Microsoft.EntityFrameworkCore.Sqlite database provider.
- Specifies directories for the resulting DbContext and model classes.