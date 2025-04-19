
using Microsoft.OpenApi.Models;
using ContosoPizza.Data;
using ContosoPizza.Services;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(opts =>
{
    opts.AddPolicy(name: MyAllowSpecificOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Registers PizzaContext with the ASP.NET Core dependency injection system.
// Specifies that PizzaContext uses the SQLite database provider.
// Defines a SQLite connection string that points to a local file, ContosoPizza.db.
// SQLite uses local database files, so it's okay to hard-code the connection string. For network databases like PostgreSQL and SQL Server, you should always store your connection strings securely. For local development, use Secret Manager. For production deployments, consider using a service like Azure Key Vault.
    // builder.Services.AddSqlite<PizzaContext>("Data Source=ContosoPizza.db");
    // builder.Services.AddSqlite<PromotionsContext>("Data Source=Promotions/Promotions.db");

// builder.Services.AddDbContext<PizzaContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<PizzaService>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // AddEndpointsApiExplorer is required only for minimal APIs. 
// Use the OpenApiInfo class to modify the information displayed in the UI:
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ContosoPizza API",
        Description = "An ASP.NET Core Web API for managing Pizza Orders & Promotions",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Example Contact",
            Url = new Uri("https://example.com/contact"),
        },
        License = new OpenApiLicense
        {
            Name = "Example License",
            Url = new Uri("https://example.com/license")
        }
    });
    // using System.Reflection;
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename)); // builds an XML file name matching that of the web API project. Some Swagger features (for example, schemata of input parameters or HTTP methods and response codes from the respective attributes) work without the use of an XML documentation file. For most features, namely method summaries and the descriptions of parameters and response codes, the use of an XML file is mandatory.
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opts =>
    {
        // serve Swagger at the root
        opts.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        opts.RoutePrefix = string.Empty;
    });
}
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

// Add the CreateDbIfNotExists method call
app.CreateDbIfNotExists(); // Calls the extension method Data/Extensions.cs each time the app runs.

app.MapGet("/", () => @"Contoso Pizza management API. Navigate to /swagger to open the Swagger test UI.");

app.Run();
