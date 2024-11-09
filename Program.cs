using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Proyecto_Herramientas.Models;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllersWithViews(); // Para controlar las vistas

builder.Services.AddDbContext<PuerBancoBd1Context>(options =>
    options.UseSqlServer("Server=JarolPc\\SQLEXPRESS;Database=PuerBancoBD1;Integrated Security=True;Persist Security Info=False;TrustServerCertificate=True"));

builder.Services.AddDistributedMemoryCache(); // Usado para almacenar datos en la memoria del servidor.
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la sesi�n.
    options.Cookie.HttpOnly = true; // Solo se accede a la cookie desde el servidor.
    options.Cookie.IsEssential = true; // Necesaria para que la sesi�n funcione sin cookies opcionales.
});
// Construir la aplicaci�n.
var app = builder.Build();



// Configurar el pipeline de manejo de solicitudes.
app.UseStaticFiles(); // Servir archivos est�ticos
app.UseRouting(); // Habilitar el enrutamiento
app.UseAuthorization(); // Habilitar autorizaci�n
app.UseSession();
// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Ruta predeterminada

// Ejecutar la aplicaci�n
app.Run();
