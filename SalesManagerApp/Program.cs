using SalesManagerApp.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDataContextConfiguration(builder.Configuration);

// Registrar a configuração do Swagger
builder.Services.AddSwaggerConfiguration();

builder.Services.AddDependencyInjectionConfiguration();

builder.Services.AddAuthConfiguration();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Usar a extensão que registra Swagger e UI corretamente
    app.UseSwaggerConfiguration();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
