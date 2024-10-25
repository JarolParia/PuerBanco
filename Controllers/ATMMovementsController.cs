using Microsoft.AspNetCore.Mvc;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMMovementsController : Controller
    {

        public IActionResult ATMMovements()
        {

            return View("~/Views/ATM/ATMMovements/ATMMovements.cshtml");
        }
    }
}

