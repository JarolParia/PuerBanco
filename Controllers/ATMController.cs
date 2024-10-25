using Microsoft.AspNetCore.Mvc;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMController : Controller
    {
        public IActionResult ATM(string accountType)
        {
            ViewData["AccountType"] = accountType; // O puedes usar directamente la sesión
            return View();
        }
    }
}
