using Microsoft.AspNetCore.Mvc;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMDepositController : Controller
    {
        public IActionResult ATMDeposit() {
            return View("~/Views/ATM/ATMDeposit/ATMDeposit.cshtml");
        }
    }
}
