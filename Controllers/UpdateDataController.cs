using Microsoft.AspNetCore.Mvc;
using Proyecto_Herramientas.Models;
using System.Text;
using System.Security.Cryptography;

namespace Proyecto_Herramientas.Controllers
{
    public class UpdateDataController : Controller
    {
        private readonly PuerBancoBd1Context _dbContext;

        public UpdateDataController(PuerBancoBd1Context context)
        {
            _dbContext = context;
        }

        public async Task<IActionResult> UpdateData()
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserData(string currentPassword, string newPassword, string firstName, string lastName, string email)
        {
            var userId = HttpContext.Session.GetInt32("UserID");
            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _dbContext.Users.FindAsync(userId);
            if (user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            bool isUpdated = false; // Variable para saber si se actualizó al menos un campo

            // Actualizar el nombre
            if (!string.IsNullOrEmpty(firstName))
            {
                user.FirstName = firstName;
                isUpdated = true;
            }

            // Actualizar el apellido
            if (!string.IsNullOrEmpty(lastName))
            {
                user.LastName = lastName;
                isUpdated = true;
            }

            // Actualizar el correo
            if (!string.IsNullOrEmpty(email))
            {
                user.Email = email;
                isUpdated = true;
            }

            // Verificar y actualizar la contraseña solo si se proporciona la contraseña actual
            if (!string.IsNullOrEmpty(currentPassword))
            {
                // Verificar la contraseña actual
                if (VerifyPassword(user.PasswordHash, currentPassword))
                {
                    // Solo actualiza si la nueva contraseña es diferente de la actual
                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        if (!VerifyPassword(user.PasswordHash, newPassword))
                        {
                            user.PasswordHash = HashPassword(newPassword);
                            isUpdated = true;
                        }
                        else
                        {
                            // Mensaje de error si la nueva contraseña es igual a la actual
                            TempData["ErrorMessage"] = "La nueva contraseña no puede ser la misma que la actual.";
                            return RedirectToAction("UpdateData"); // Retornar la vista para mostrar el mensaje
                        }
                    }
                }
                else
                {
                    // Mensaje de error si la contraseña actual es incorrecta
                    TempData["ErrorMessage"] = "La contraseña actual es incorrecta.";
                    return RedirectToAction("UpdateData"); // Retornar la vista para mostrar el mensaje
                }
            }

            // Guardar los cambios en la base de datos
            await _dbContext.SaveChangesAsync();

            // Si al menos un campo fue actualizado, mostrar el mensaje de éxito
            if (isUpdated)
            {
                TempData["SuccessMessage"] = "¡Datos actualizados correctamente!";
            }
            else
            {
                TempData["SuccessMessage"] = "No se realizaron cambios en los datos.";
            }

            // Redirigir a la vista de actualización
            return RedirectToAction("UpdateData");
        }

        private byte[] HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
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
    }
}
