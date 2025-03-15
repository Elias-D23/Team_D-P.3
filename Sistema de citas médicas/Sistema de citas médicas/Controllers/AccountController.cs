using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas_.Data;
using Sistema_de_citas_médicas_.Models;
using Sistema_de_citas_médicas_.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace Sistema_de_citas_médicas_.Controllers
{
    public class AccountController : Controller
    {
        private readonly CitaMedicaContext _context;

        public AccountController(CitaMedicaContext context)
        {
            _context = context;
        }

        // GET: /Account/Registro
        public IActionResult Registro()
        {
            return View();
        }

        // POST: /Account/Registro
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(RegistroViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verificar si ya existe un usuario con el mismo correo electrónico
                var clienteExistente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.CorreoElectronico == model.CorreoElectronico);

                if (clienteExistente != null)
                {
                    ModelState.AddModelError("CorreoElectronico", "Este correo electrónico ya está registrado.");
                    return View(model);
                }

                // Verificar si ya existe un usuario con la misma cédula
                var clienteConCedula = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.Cedula == model.Cedula);

                if (clienteConCedula != null)
                {
                    ModelState.AddModelError("Cedula", "Esta cédula ya está registrada.");
                    return View(model);
                }

                // Crear un nuevo cliente
                var cliente = new Cliente
                {
                    Nombre = model.Nombre,
                    Cedula = model.Cedula,
                    FechaNacimiento = model.FechaNacimiento,
                    Telefono = model.Telefono,
                    CorreoElectronico = model.CorreoElectronico,
                    Contrasena = model.Contrasena // En una aplicación real, deberías hashear la contraseña
                };

                // Guardar el cliente en la base de datos
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();

                // Iniciar sesión automáticamente
                HttpContext.Session.SetInt32("ClienteId", cliente.id);
                HttpContext.Session.SetString("NombreCliente", cliente.Nombre);

                TempData["Mensaje"] = "¡Registro exitoso! Bienvenido/a " + cliente.Nombre;
                return RedirectToAction("Index", "Home");
            }

            // Si llegamos aquí, algo falló, redisplaya la forma
            return View(model);
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Buscar usuario por correo electrónico
                var cliente = await _context.Clientes
                    .FirstOrDefaultAsync(c => c.CorreoElectronico == model.CorreoElectronico);

                if (cliente == null || cliente.Contrasena != model.Contrasena)
                {
                    ModelState.AddModelError("", "Correo electrónico o contraseña incorrectos.");
                    return View(model);
                }

                // Iniciar sesión
                HttpContext.Session.SetInt32("ClienteId", cliente.id);
                HttpContext.Session.SetString("NombreCliente", cliente.Nombre);

                TempData["Mensaje"] = "¡Bienvenido/a de nuevo, " + cliente.Nombre + "!";
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        // GET: /Account/Logout
        public IActionResult Logout()
        {
            // Cerrar sesión
            HttpContext.Session.Clear();

            TempData["Mensaje"] = "Has cerrado sesión correctamente.";
            return RedirectToAction("Index", "Home");
        }
    }
}