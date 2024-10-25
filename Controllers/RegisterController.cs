using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Herramientas.Controllers
{
    public class RegisterController : Controller
    {
        public IActionResult Register() { 
            return View();
        }
    }
}
