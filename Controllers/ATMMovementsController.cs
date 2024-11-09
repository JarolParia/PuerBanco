using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Herramientas.Models;
using Proyecto_Herramientas.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Herramientas.Controllers
{
    public class ATMMovementsController : Controller
    {
        private readonly PuerBancoBd1Context _dbContext;

        public ATMMovementsController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }

        public async Task<IActionResult> ATMMovements()
        {
            var accountNumber = HttpContext.Session.GetInt32("AccountNumber");

            if (accountNumber == null)
            {
                TempData["Error"] = "Por favor, selecciona una cuenta primero.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Obtener la cuenta
            var account = await _dbContext.Accounts
                .Include(a => a.Type)
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber.Value);

            if (account == null)
            {
                TempData["Error"] = "No se encontró la cuenta.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Obtener el usuario
            var userId = HttpContext.Session.GetInt32("UserID");
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
            {
                TempData["Error"] = "No se encontró el usuario.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Obtener los movimientos de la cuenta
            var movements = _dbContext.Movements
            .Include(m => m.OriginAccountNavigation)
            .Include(m => m.DestinyAccountNavigation)
            .Include(m => m.TransactionT)
            .Where(m => m.OriginAccount == accountNumber || m.DestinyAccount == accountNumber)
            .ToList();

            var viewModel = new ATMViewModel
            {
                Account = account,
                User = user,
                Movements = movements,
                AccountNumberL = accountNumber.Value
            };

            return View("~/Views/ATM/ATMMovements/ATMMovements.cshtml", viewModel);
        }
    }
}
