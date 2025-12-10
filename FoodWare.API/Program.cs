using FoodWare.Shared.Interfaces;
using FoodWare.API.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Inyección de Dependencias: Registramos TODOS los repositorios
builder.Services.AddScoped<IMovimientoRepository, MovimientoSqlRepository>();
builder.Services.AddScoped<IProductoRepository, ProductoSqlRepository>();
builder.Services.AddScoped<IVentaRepository, VentaSqlRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioSqlRepository>();
builder.Services.AddScoped<IRecetaRepository, RecetaSqlRepository>();
builder.Services.AddScoped<IFinanzasRepository, FinanzasSqlRepository>();
builder.Services.AddScoped<IRolRepository, RolSqlRepository>();
// --- AGREGADO: Repositorio de Platillos ---
builder.Services.AddScoped<IPlatilloRepository, PlatilloSqlRepository>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
await app.RunAsync();
