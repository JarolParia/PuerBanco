using Microsoft.AspNetCore.Mvc;
using Proyecto_Herramientas.Models;

namespace Proyecto_Herramientas.Controllers
{
    public class MenuController : Controller
    {
        private readonly PuerBancoBd1Context _dbContext;

        public MenuController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }

        // Acción para la vista del menú
        public async Task<IActionResult> Menu()
        {
            // Obtener el ID y el rol del usuario desde la sesión
            var userId = HttpContext.Session.GetInt32("UserID");
            var userRole = HttpContext.Session.GetString("UserRole");

            // Verificar si hay una sesión activa
            if (userId == null || userRole == null)
            {
                // Si no hay sesión, redirigir al login
                return RedirectToAction("Index", "Home");
            }

            // Cargar información adicional del usuario para personalizar la vista, si es necesario
            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                // Redirige al login si el usuario no existe en la base de datos (por ejemplo, si fue eliminado)
                return RedirectToAction("Index", "Home");
            }

            // Pasar los datos del usuario a la vista para poder personalizar el menú
            return View(user);
        }
    }
}
