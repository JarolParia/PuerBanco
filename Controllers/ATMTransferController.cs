using Microsoft.AspNetCore.Mvc;
namespace Proyecto_Herramientas.Controllers
{
    public class ATMTransferController : Controller
    {
        public IActionResult ATMTransfer() {
            return View("~/Views/ATM/ATMTransfer/ATMTransfer.cshtml");
        }
    }
}
