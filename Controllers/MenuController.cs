using Microsoft.AspNetCore.Mvc;

namespace Proyecto_Herramientas.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult Menu() { 
        return View();
        }
    }
}
