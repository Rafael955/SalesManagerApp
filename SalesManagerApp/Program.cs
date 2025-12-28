using SalesManagerApp.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Registrar a configuração do Swagger
builder.Services.AddSwaggerConfiguration();

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddDependencyInjectionConfiguration();

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
