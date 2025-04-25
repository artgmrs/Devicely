using Devicely.Database;
using Devicely.Application;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add modules
builder.Services.AddDevicelyDatabaseModule(builder.Configuration);
builder.Services.AddDevicelyApplicationModule();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Devicely",
        Description = "The API that manages devices so well, you'll think it uses AI :)",
        Contact = new OpenApiContact
        {
            Name = "Arthur Miranda",
            Email = "artgmrs@outlook.com",
            Url = new Uri("https://artgmrs-card.vercel.app/"),
        },
    });

    options.EnableAnnotations();

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
