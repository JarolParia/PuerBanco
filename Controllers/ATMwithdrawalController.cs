using Microsoft.AspNetCore.Mvc;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMwithdrawalController : Controller
    {
        public IActionResult ATMwithdrawal() {
            return View("~/Views/ATM/ATMwithdrawal/ATMwithdrawal.cshtml");
        }
    }
}
