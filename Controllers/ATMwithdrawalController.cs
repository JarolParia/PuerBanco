using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Herramientas.Models;
using Proyecto_Herramientas.ViewModels;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMwithdrawalController : Controller
    {

        private readonly PuerBancoBd1Context _dbContext;

        public ATMwithdrawalController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }

        public IActionResult ATMwithdrawal()
        {
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


            return View("~/Views/ATM/ATMwithdrawal/ATMwithdrawal.cshtml", account);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> withdrawal(decimal amount)
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
                TempData["Error"] = "El monto a retirar debe ser mayor que 0.";
                return View("~/Views/ATM/ATMwithdrawal/ATMwithdrawal.cshtml", account);
            }

            // Verificar el saldo según el tipo de cuenta
            if (account.TypeId == 1 && amount > account.Balance)
            {
                // Tipo 1: cuenta regular, sin descubierto permitido
                TempData["Error"] = "Saldo insuficiente para realizar el retiro.";
                return View("~/Views/ATM/ATMwithdrawal/ATMwithdrawal.cshtml", account);
            }
            else if (account.TypeId == 2 && (account.Balance - amount) < -50000)
            {
                // Tipo 2: cuenta de checking, con descubierto máximo de 50,000
                TempData["Error"] = "El retiro excede el descubierto permitido de 50,000.";
                return View("~/Views/ATM/ATMwithdrawal/ATMwithdrawal.cshtml", account);
            }

            // Crear un nuevo movimiento para el retiro
            var movement = new Movement
            {
                DestinyAccount = accountNumber,
                TransactionTId = 1,  // Asumiendo que 1 es el ID para "Retiro"
                Amount = amount,
                TransactionDate = DateTime.Now
            };

            // Actualizar el saldo de la cuenta
            account.Balance -= amount;

            // Agregar el movimiento a la cuenta
            account.MovementDestinyAccountNavigations.Add(movement);

            // Guardar los cambios en la base de datos
            await _dbContext.SaveChangesAsync();

            // Redirigir a una vista de éxito o a la página de saldo
            TempData["Success"] = "Retiro realizado con éxito.";
            return View("~/Views/ATM/ATMwithdrawal/ATMwithdrawal.cshtml", account);
        }

    }
}
