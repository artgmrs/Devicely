using Devicely.Database;
using Devicely.Application;

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

var app = builder.Build();

// Configure swagger
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
