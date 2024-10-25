using Microsoft.AspNetCore.Mvc;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMBalanceController : Controller
    {
        public IActionResult ATMBalance() {
            return View("~/Views/ATM/ATMBalance/ATMBalance.cshtml");
        }
    }
}
