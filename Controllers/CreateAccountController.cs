using Microsoft.AspNetCore.Mvc;
using Proyecto_Herramientas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Proyecto_Herramientas.Controllers
{
    public class CreateAccountController : Controller
    {
        private readonly PuerBancoBd1Context _context;

        public CreateAccountController(PuerBancoBd1Context context)
        {
            _context = context;
        }

        // GET: Account/Create
        public IActionResult CreateAccount()
        {
            return View();
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int typeId)
        {

            if (typeId != 1 && typeId != 2)
            {
                // Agrega un mensaje de error que se mostrará en la vista
                ModelState.AddModelError("typeId", "Debe seleccionar un tipo de cuenta.");
                return View("CreateAccount"); // Devuelve la misma vista con el mensaje de error
            }

            // Obtiene el userId de la sesión
            var userId = HttpContext.Session.GetInt32("UserID");

            // Verifica si el userId existe en la sesión
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Genera un número de cuenta único
            var random = new Random();
            int newAccountNumber;
            do
            {
                newAccountNumber = random.Next(100000000, 999999999); // Ajusta el rango según tu formato de cuenta
            }
            while (_context.Accounts.Any(a => a.AccountNumber == newAccountNumber));

            var user = await _context.Users.Include(u => u.Accounts).FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null) return NotFound();

            // Crear una nueva instancia de Account
            var account = new Account
            {
                AccountNumber = newAccountNumber,
                UserId = userId.Value,
                TypeId = typeId,
                Balance = 0.0m,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            // Asignar valores específicos dependiendo del tipo de cuenta
            if (typeId == 1) // Si es una cuenta de ahorro (SavingAccount)
            {
                var savingAccount = new SavingAccount
                {
                    AccountNumber = newAccountNumber,
                    InterestRate = 10.0m // 10% de interés
                };
                account.SavingAccount = savingAccount; // Asocia la cuenta de ahorros con la cuenta general
            }
            else if (typeId == 2) // Si es una cuenta corriente (CheckingAccount)
            {
                var checkingAccount = new CheckingAccount
                {
                    AccountNumber = newAccountNumber,
                    OverDraftLimit = 50000.0m // Límite de sobregiro de 50,000
                };
                account.CheckingAccount = checkingAccount; // Asocia la cuenta corriente con la cuenta general
            }
            // Agrega la cuenta y guarda los cambios en la base de datos
            user.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return RedirectToAction("Menu", "Menu");
        }
    }
}
