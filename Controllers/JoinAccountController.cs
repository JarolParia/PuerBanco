using Microsoft.AspNetCore.Mvc;
using Proyecto_Herramientas.Models;
using Proyecto_Herramientas.ViewModels;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Proyecto_Herramientas.Controllers
{
    public class JoinAccountController : Controller
    {
        private readonly PuerBancoBd1Context _context;

        public JoinAccountController(PuerBancoBd1Context context)
        {
            _context = context;
        }

        // GET: Account/JoinAccount
        public async Task<IActionResult> JoinAccount()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _context.Users
                .Include(u => u.Accounts) // Esto incluye las cuentas relacionadas con el usuario
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult EnterAccount(int accountNumber)
        {
            var account = _context.Accounts
                .Include(a => a.Type)
                .FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                TempData["Error"] = "La cuenta seleccionada no existe.";
                return RedirectToAction("JoinAccount");
            }

            if (account.Type == null)
            {
                TempData["Error"] = "El tipo de cuenta no está definido.";
                return RedirectToAction("JoinAccount");
            }

            // Verificar si la cuenta está activa
            if (!account.IsActive) // Asegúrate de que esta propiedad exista en tu modelo
            {
                TempData["Error"] = "La cuenta seleccionada no está activa.";
                return RedirectToAction("JoinAccount");
            }

            // Guardar el número de cuenta en la sesión
            HttpContext.Session.SetInt32("AccountNumber", accountNumber);

            // Redirigir al cajero
            return RedirectToAction("ATM", "ATM");
        }





    }
}
