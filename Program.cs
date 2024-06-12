using Models;
using Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ProveedoresDbSettings>(builder.Configuration.GetSection("proveedoresDbSettings"));
builder.Services.AddSingleton<ProveedoresServices>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/proveedores", async (ProveedoresServices proveedoresServices) =>
{
    return await proveedoresServices.listaProveedores();
});

app.MapGet("/proveedores/{id}", async (ProveedoresServices proveedoresServices, string id) =>
{
    return await proveedoresServices.ObtenerProveedor(id);
});

app.MapPost("/proveedores", async (ProveedoresServices proveedoresServices, Proveedores proveedor) =>
{
    return await proveedoresServices.CrearProveedor(proveedor);
});

app.MapPut("/proveedores/{id}", async (ProveedoresServices proveedoresServices, string id, Proveedores proveedor) =>
{
    return await proveedoresServices.ActualizarProveedor(id, proveedor);
});

app.MapDelete("/proveedores/{id}", async (ProveedoresServices proveedoresServices, string id) =>
{
    return await proveedoresServices.EliminarProveedor(id);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
