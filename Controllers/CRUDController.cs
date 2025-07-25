﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_Herramientas.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text;
namespace Proyecto_Herramientas.Controllers
{
    public class CRUDController : Controller
    {
        private readonly PuerBancoBd1Context _context;

        public CRUDController(PuerBancoBd1Context context)
        {
            _context = context;
        }
        public async Task<IActionResult> CRUD()
        {
            var usersAndAccounts = await _context.Users
                .Where(u => u.UserType == "Client")
                .Select(u => new
                {
                    UserID = u.UserId,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserType = u.UserType,
                    Email = u.Email,
                    HasAccounts = u.Accounts.Any(),
                    Accounts = u.Accounts.Select(a => new
                    {
                        AccountNumber = a.AccountNumber,
                        AccountType = _context.AccountTypes
                            .Where(at => at.TypeId == a.TypeId)
                            .Select(at => at.TypeName)
                            .FirstOrDefault() ?? "Sin tipo",
                        Balance = a.Balance,
                        IsActive = a.IsActive
                    }).ToList()
                })
                .ToListAsync();

            var admin = await _context.Users.FirstOrDefaultAsync(u => u.UserType == "Admin");
            string adminName = admin != null ? admin.FirstName + " " + admin.LastName : "Admin";
            ViewData["AdminName"] = adminName;

            var users = await _context.Users.ToListAsync(); 
            // Pasar los usuarios a la vista usando ViewData
            ViewData["Users"] = users;

            return View(usersAndAccounts);
        }

        [HttpGet]
        public async Task<IActionResult> GetChangesHistory()
        {
            try
            {
                var changesHistory = await _context.ChangesHistories
                    .Include(ch => ch.User)  // Asumiendo que tienes la relación definida en el modelo
                    .OrderByDescending(ch => ch.ChangeDate)
                    .Select(ch => new
                    {
                        ch.ChangesId,
                        UserName = $"{ch.User.FirstName} {ch.User.LastName}",
                        ChangeDate = ch.ChangeDate.ToString("dd/MM/yyyy HH:mm:ss"),
                        ch.ChangeType,
                        ch.TableAffected,
                        ch.RecordId,
                        ch.Description
                    })
                    .ToListAsync();

                return Json(changesHistory);
            }
            catch (Exception ex)
            {
                return Json(new { error = "Error al obtener el historial de cambios" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ToggleAccountStatus(int accountNumber)
        {
            int? userId = HttpContext.Session.GetInt32("UserID"); // Obtener userId de la sesión
            if (userId == null)
            {
                return RedirectToAction("Index", "Home"); // Redirige si no hay userId en la sesión
            }

            var account = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == accountNumber);
            if (account != null)
            {
                // Guardar el estado anterior
                bool previousStatus = account.IsActive;

                // Cambia el estado de activo a inactivo y viceversa
                account.IsActive = !account.IsActive;
                await _context.SaveChangesAsync(); // Guarda los cambios en la base de datos

                // Crear un nuevo registro en ChangesHistory
                var changeHistory = new ChangesHistory
                {
                    UserId = userId.Value, // Asegúrate de que el nombre de la propiedad sea correcto
                    ChangeDate = DateTime.Now,
                    ChangeType = previousStatus ? "Deactivate" : "Activate", // Cambia el tipo de cambio
                    TableAffected = "Accounts",
                    RecordId = account.AccountNumber, // ID del registro afectado
                    Description = $"Account {account.AccountNumber} has been {(previousStatus ? "deactivated" : "activated")}."
                };

                _context.ChangesHistories.Add(changeHistory);
                await _context.SaveChangesAsync(); // Guarda el registro de cambios
                return RedirectToAction("CRUD");
            }
            // Redirige a la vista principal
            return RedirectToAction("Menu", "Menu");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(int userId, string firstName, string lastName, string email)
        {
            int? userIdd = HttpContext.Session.GetInt32("UserID"); // Obtener userId de la sesión

            if (!userIdd.HasValue)
            {
                return RedirectToAction("Index", "Home"); // Redirige si no hay userId en la sesión
            }

            // Buscar al usuario por el ID
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Guardar el estado anterior para verificar cambios
            var oldFirstName = user.FirstName;
            var oldLastName = user.LastName;
            var oldEmail = user.Email;

            // Actualizar los datos del usuario
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Email = email;

            await _context.SaveChangesAsync(); // Guardar cambios en la tabla de usuarios

            // Construir la descripción solo con los campos que cambiaron
            var descriptionBuilder = new StringBuilder("Updated profile:");
            bool hasChanges = false;

            if (oldFirstName != firstName)
            {
                descriptionBuilder.Append($" FirstName: {oldFirstName} -> {firstName};");
                hasChanges = true;
            }
            if (oldLastName != lastName)
            {
                descriptionBuilder.Append($" LastName: {oldLastName} -> {lastName};");
                hasChanges = true;
            }
            if (oldEmail != email)
            {
                descriptionBuilder.Append($" Email: {oldEmail} -> {email};");
                hasChanges = true;
            }

            // Si hubo cambios, crear el registro en ChangesHistory
            if (hasChanges)
            {
                var changeHistory = new ChangesHistory
                {
                    UserId = userIdd.Value, // ID del usuario que hizo el cambio
                    ChangeDate = DateTime.Now,
                    ChangeType = "Change Profile", // Tipo de cambio
                    TableAffected = "Users",
                    RecordId = userId, // ID del registro afectado
                    Description = descriptionBuilder.ToString().TrimEnd(';') // Guardar solo cambios específicos
                };

                _context.ChangesHistories.Add(changeHistory);
                await _context.SaveChangesAsync(); // Guarda el registro de cambios
            }

            // Redirigir a la vista que lista los usuarios (ejemplo: "CRUD")
            return RedirectToAction("CRUD");
        }

    }
}
