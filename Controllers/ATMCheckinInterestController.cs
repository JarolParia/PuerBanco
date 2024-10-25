using Microsoft.AspNetCore.Mvc;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMCheckinInterestController : Controller
    {
        public IActionResult ATMCheckinInterest() {
            return View("~/Views/ATM/ATMCheckinInterest/ATMCheckinInterest.cshtml");
        }
    }
}
