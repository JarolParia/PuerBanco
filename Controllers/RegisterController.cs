using Microsoft.AspNetCore.Mvc;
using Proyecto_Herramientas.Models;
using System.Text;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

using System.Security.Cryptography;
namespace Proyecto_Herramientas.Controllers
{
    public class RegisterController : Controller
    {
        private readonly PuerBancoBd1Context _dbContext;

        public RegisterController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public IActionResult SignIn(string email, string firstname, string lastname, string password, string confirmPassword)
        {
            // Validación de longitud de contraseña
            if (!System.Text.RegularExpressions.Regex.IsMatch(password, @"^\d{4}$"))
            {
                ModelState.AddModelError("Password", "La contraseña debe ser solo numérica y tener exactamente 4 dígitos.");
            }

            // Verificación de coincidencia de contraseñas
            if (password != confirmPassword)
            {
                ModelState.AddModelError("ConfirmPassword", "Las contraseñas no coinciden.");
            }

            // Comprobar si el correo ya está registrado
            if (_dbContext.Users.Any(u => u.Email == email))
            {
                ModelState.AddModelError("Email", "El correo electrónico ya está en uso");
                return View("Register");
            }

            // Comprobar si el modelo tiene errores antes de proceder
            if (!ModelState.IsValid)
            {
                // Retornar la vista con los errores del modelo
                return View("Register");
            }

            // Generar hash de la contraseña
            byte[] passwordHash;
            using (var sha256 = SHA256.Create())
            {
                passwordHash = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }

            // Crear nuevo usuario
            var user = new User
            {
                Email = email,
                FirstName = firstname,
                LastName = lastname,
                PasswordHash = passwordHash,
                UserType = "Client", // Asigna un tipo de usuario predeterminado
                CreatedAt = DateTime.Now
            };

            // Guardar en la base de datos
            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Agregar un mensaje de error genérico si ocurre una excepción
                ModelState.AddModelError(string.Empty, "Ocurrió un error al procesar tu registro. Intenta de nuevo más tarde.");
                // Aquí podrías registrar el error (variable ex) en un sistema de logging si tienes uno
                return View("Register");
            }

            // Redirigir al inicio después del registro
            return RedirectToAction("Index", "Home");
        }




    }
}
