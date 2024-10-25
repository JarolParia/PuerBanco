using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews(); // Para controlar las vistas
builder.Services.AddSession(); // Habilitar sesiones
builder.Services.AddHttpContextAccessor();
// Construir la aplicación.
var app = builder.Build();

// Usar sesiones
app.UseSession();

// Configurar el pipeline de manejo de solicitudes.
app.UseStaticFiles(); // Servir archivos estáticos
app.UseRouting(); // Habilitar el enrutamiento
app.UseAuthorization(); // Habilitar autorización

// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Ruta predeterminada

// Ejecutar la aplicación
app.Run();
