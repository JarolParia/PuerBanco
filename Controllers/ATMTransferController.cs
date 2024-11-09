using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Herramientas.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Herramientas.Controllers
{
    public class ATMTransferController : Controller
    {
        private readonly PuerBancoBd1Context _dbContext;

        public ATMTransferController(PuerBancoBd1Context dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult ATMTransfer()
        {
            var accountNumber = HttpContext.Session.GetInt32("AccountNumber");

            if (accountNumber == null)
            {
                TempData["Error"] = "Por favor, selecciona una cuenta primero.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Verifica si la cuenta es una CheckingAccount
            var account = _dbContext.Accounts
                .FirstOrDefault(a => a.AccountNumber == accountNumber);

            if (account == null)
            {
                TempData["Error"] = "Esta cuenta no permite transferencias.";
                return RedirectToAction("ATMBalance", "ATMBalance");
            }

            // Pasa los datos necesarios a la vista
            return View("~/Views/ATM/ATMTransfer/ATMTransfer.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(int DestinyAccountNumber, decimal amount)
        {
            var originAccountNumber = HttpContext.Session.GetInt32("AccountNumber");

            if (originAccountNumber == null)
            {
                TempData["Error"] = "Por favor, selecciona una cuenta primero.";
                return RedirectToAction("JoinAccount", "JoinAccount");
            }

            // Obtener cuentas de origen y destino
            var originAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == originAccountNumber);

            var destinyAccount = await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == DestinyAccountNumber);

            if (originAccount == null || destinyAccount == null)
            {
                TempData["Error"] = "Cuenta de origen o destino no encontrada.";
                return View("~/Views/ATM/ATMTransfer/ATMTransfer.cshtml");
            }

            // Verificar que la cuenta de destino sea diferente de la cuenta de origen
            if (originAccount.AccountNumber == destinyAccount.AccountNumber)
            {
                TempData["Error"] = "La cuenta de destino no puede ser la misma que la cuenta de origen.";
                return View("~/Views/ATM/ATMTransfer/ATMTransfer.cshtml");
            }

            // Validaciones de saldo y monto
            if (amount <= 0)
            {
                TempData["Error"] = "El monto de la transferencia debe ser mayor a 0.";
                return View("~/Views/ATM/ATMTransfer/ATMTransfer.cshtml");
            }

            if (originAccount.Balance < amount)
            {
                TempData["Error"] = "Fondos insuficientes para completar la transferencia.";
                return View("~/Views/ATM/ATMTransfer/ATMTransfer.cshtml");
            }

            // Crear movimientos de transferencia para origen y destino
            var movement = new Movement
            {
                OriginAccount = originAccountNumber,
                DestinyAccount = destinyAccount.AccountNumber,
                TransactionTId = 3, // ID para transferencias
                Amount = amount, // Monto positivo para el movimiento de salida
                TransactionDate = DateTime.Now
            };



            // Actualizar saldos
            originAccount.Balance -= amount;
            destinyAccount.Balance += amount;

            // Guardar movimientos y cambios de saldo
            await _dbContext.Movements.AddAsync(movement);
            await _dbContext.SaveChangesAsync();

            TempData["Success"] = $"Transferencia realizada con éxito a la cuenta {DestinyAccountNumber}.";
            return View("~/Views/ATM/ATMTransfer/ATMTransfer.cshtml");
        }


    }
}