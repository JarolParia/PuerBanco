using Microsoft.AspNetCore.Mvc;
using Proyecto_Herramientas.Models;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Azure.Core;
using Microsoft.AspNetCore.Http;

namespace Proyecto_Herramientas.Controllers
{
    public class HomeController : Controller
    {
        private readonly PuerBancoBd1Context _dbContext;

        public HomeController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            // Buscar el usuario en la base de datos por su correo electrónico
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            // Verificar si el usuario existe y si la contraseña es correcta
            if (user == null || !VerifyPassword(user.PasswordHash, password))
            {
                ModelState.AddModelError(string.Empty, "Credenciales incorrectas.");
                return View("Index");
            }

            // Si el usuario existe y la contraseña es correcta
            if (user.UserType == "Client")
            {
                // Guardar el UserID y UserType en la sesión
                HttpContext.Session.SetInt32("UserID", user.UserId); // Almacenar el ID del usuario
                HttpContext.Session.SetString("UserRole", user.UserType); // Almacenar el rol del usuario

                // Redirigir a la página principal de cliente (Menu)
                return RedirectToAction("Menu", "Menu");
            }
            else if (user.UserType == "Admin")
            {
                // Guardar el UserID y UserType en la sesión
                HttpContext.Session.SetInt32("UserID", user.UserId);
                HttpContext.Session.SetString("UserRole", user.UserType);

                // Redirigir a una página de administración o registro, según tu lógica
                return RedirectToAction("CRUD", "CRUD");
            }

            // En caso de que el tipo de usuario no sea reconocido, redirige al login
            return RedirectToAction("Index");
        }

        public IActionResult logout()
        {
            HttpContext.Session.Clear(); // Elimina todos los datos de la sesión
            return RedirectToAction("Index", "Home"); // Redirige al login o inicio
        }

        private bool VerifyPassword(byte[] storedHash, string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPassword = sha256.ComputeHash(passwordBytes);
                return hashedPassword.SequenceEqual(storedHash);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}