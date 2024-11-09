using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Herramientas.Models;
using Proyecto_Herramientas.ViewModels;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMBalanceController : Controller
    {
        private readonly PuerBancoBd1Context _dbContext;

        public ATMBalanceController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }
        public IActionResult ATMBalance()
        {
            var accountNumber = HttpContext.Session.GetInt32("AccountNumber");

            if (accountNumber == null)
            {
                TempData["Error"] = "Por favor, selecciona una cuenta primero.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Obtener la cuenta con el número de cuenta, incluyendo las cuentas específicas
            var account = _dbContext.Accounts
                .Include(a => a.SavingAccount)    // Incluir SavingAccount si es de ahorros
                .Include(a => a.CheckingAccount)  // Incluir CheckingAccount si es corriente
                .FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                TempData["Error"] = "No se encontró la cuenta.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            var userId = HttpContext.Session.GetInt32("UserID");
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);

            // Crear el ViewModel con la cuenta y el usuario
            var viewModel = new ATMViewModel
            {
                Account = account,
                User = user,
                CheckingAccount = account.CheckingAccount,  // Solo se asigna si es CheckingAccount
                SavingAccount = account.SavingAccount       // Solo se asigna si es SavingAccount
            };

            return View("~/Views/ATM/ATMBalance/ATMBalance.cshtml", viewModel);
        }

    }
}
