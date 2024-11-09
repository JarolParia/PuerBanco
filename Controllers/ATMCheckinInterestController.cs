using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Herramientas.Models;
using Proyecto_Herramientas.ViewModels;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMCheckinInterestController : Controller
    {

        private readonly PuerBancoBd1Context _dbContext;

        public ATMCheckinInterestController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }

        public IActionResult ATMCheckinInterest() {
            var accountNumber = HttpContext.Session.GetInt32("AccountNumber");

            if (accountNumber == null)
            {
                TempData["Error"] = "Por favor, selecciona una cuenta primero.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Obtener la cuenta con el número de cuenta, incluyendo las cuentas específicas
            var account = _dbContext.Accounts
                .Include(a => a.SavingAccount) // Solo cargar la cuenta de ahorros si existe
                .Include(a => a.CheckingAccount) // Cargar la cuenta corriente también si existe
                .FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                TempData["Error"] = "No se encontró la cuenta.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            var userId = HttpContext.Session.GetInt32("UserID");
            var user = _dbContext.Users.FirstOrDefault(u => u.UserId == userId);

            // Calcular el interés solo si es una cuenta de ahorros
            if (account.SavingAccount != null)
            {
                decimal tasaInteres = 0.10m;  // 10% de interés fijo
                decimal interesGenerado = account.Balance * tasaInteres;  // Calculamos el interés

                // Actualizamos el saldo final después de sumar el interés
                decimal saldoFinal = account.Balance + interesGenerado;

                // Guardar los valores en el ViewModel
                ViewBag.InteresGenerado = interesGenerado;
                ViewBag.SaldoFinal = saldoFinal;
            }

            // Crear el ViewModel con la cuenta y el usuario
            var viewModel = new ATMViewModel
            {
                Account = account,
                User = user,
                CheckingAccount = account.CheckingAccount,
                SavingAccount = account.SavingAccount
            };
            return View("~/Views/ATM/ATMCheckinInterest/ATMCheckinInterest.cshtml", viewModel);
        }
    }
}
