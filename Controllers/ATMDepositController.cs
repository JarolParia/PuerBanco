using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Herramientas.Models;
using Proyecto_Herramientas.ViewModels;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMDepositController : Controller
    {

        private readonly PuerBancoBd1Context _dbContext;

        public ATMDepositController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }

        public IActionResult ATMDeposit() {
            var accountNumber = HttpContext.Session.GetInt32("AccountNumber");

            if (accountNumber == null)
            {
                TempData["Error"] = "Por favor, selecciona una cuenta primero.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Obtener la cuenta con el número de cuenta
            var account = _dbContext.Accounts
                .FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                TempData["Error"] = "No se encontró la cuenta.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }


            return View("~/Views/ATM/ATMDeposit/ATMDeposit.cshtml",account);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(decimal amount)
        {
            var accountNumber = HttpContext.Session.GetInt32("AccountNumber");

            if (accountNumber == null)
            {
                TempData["Error"] = "Por favor, selecciona una cuenta primero.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Obtener la cuenta con el número de cuenta
            var account = await _dbContext.Accounts
                .Include(a => a.MovementDestinyAccountNavigations) // Incluye los movimientos de la cuenta
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                TempData["Error"] = "No se encontró la cuenta.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Validar que el monto sea mayor que 0
            if (amount <= 0)
            {
                TempData["Error"] = "El monto a depositar debe ser mayor que 0.";
                return RedirectToAction("~/Views/ATM/ATMDeposit/ATMDeposit.cshtml");
            }

            // Crear un nuevo movimiento para el depósito
            var movement = new Movement
            {
                DestinyAccount = accountNumber,
                TransactionTId = 2,  // Asumiendo que 2 es el ID para "Depósito"
                Amount = amount,
                TransactionDate = DateTime.Now
            };

            // Actualizar el saldo de la cuenta
            account.Balance += amount;

            // Agregar el movimiento a la cuenta
            account.MovementDestinyAccountNavigations.Add(movement);

            // Guardar los cambios en la base de datos
            await _dbContext.SaveChangesAsync();

            // Redirigir a una vista de éxito o a la página de saldo
            TempData["Success"] = "Depósito realizado con éxito.";
            return View("~/Views/ATM/ATMDeposit/ATMDeposit.cshtml");
        }

    }
}
