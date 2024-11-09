using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Herramientas.Models;
using Proyecto_Herramientas.ViewModels;

namespace Proyecto_Herramientas.Controllers
{
    public class ATMController : Controller
    {
        private readonly PuerBancoBd1Context _dbContext;

        public ATMController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }

        public IActionResult ATM()
        {
            // Obtener el número de cuenta de la sesión
            var accountNumber = HttpContext.Session.GetInt32("AccountNumber");

            if (accountNumber == null)
            {
                TempData["Error"] = "Por favor, selecciona una cuenta primero.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            var account = _dbContext.Accounts
                .Include(a => a.Type)
                .FirstOrDefault(a => a.AccountNumber == accountNumber.Value);

            var userId = HttpContext.Session.GetInt32("UserID");
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);

            if (account == null || user == null)
            {
                TempData["Error"] = "Información de cuenta o usuario no válida.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            var viewModel = new ATMViewModel
            {
                Account = account,
                User = user
            };

            return View("ATM", viewModel);
        }
    }
}
