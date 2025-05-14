
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

// dotnet user-secrets set "Movies:ServiceApiKey" "12345": JSON structure { Movies: { ServiceApiKey: "12345" } }
// Connection String Syntax: "Server=<SQLServerAddress|localhost>;<Initial Catalog|Database>=<DataBaseName|ContosoPizza>;User Id=Username;Password=UserPassword;"
// var connectionString = builder.Configuration.GetConnectionString("ContosoPizzaConn")
//         ?? throw new InvalidOperationException("Connection string: "
//         + builder.Configuration.GetConnectionString("ContosoPizzaConn") ?? "ContosoPizzaConn" + " not found.");

// using env var
DotNetEnv.Env.Load();
string connectionString = Environment.GetEnvironmentVariable("CONTOSO_PIZZA_CONN") ?? throw new InvalidOperationException("Connection string not found.");

builder.Services.AddDbContext<PizzaContext>(options =>
    options.UseSqlServer(
        connectionString,
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()
    ));

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

    // Steps for calling seed logic
    // using var scope = app.Services.CreateScope();
    // var context = scope.ServiceProvider.GetRequiredService<PizzaContext>();
    // await DbInitializer.Initialize(context);
    await app.CreateDbIfNotExists(); // Calls the extension method Data/Extensions.cs each time the app runs.
}
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => @"Contoso Pizza management API. Navigate to /swagger to open the Swagger test UI.");
app.Run();
