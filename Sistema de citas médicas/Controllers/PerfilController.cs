using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas.Data;
using Sistema_de_citas_médicas.Models;
using Sistema_de_citas_médicas.Models.ViewModels;
using System;
using System.Threading.Tasks;

namespace Sistema_de_citas_médicas.Controllers
{
    public class PerfilController : Controller
    {
        private readonly CitaMedicaContext _context;

        public PerfilController(CitaMedicaContext context)
        {
            _context = context;
        }

        // GET: Perfil/Editar
        public async Task<IActionResult> Editar()
        {
            // Obtener el ID del usuario de la sesión
            int clienteId = ObtenerClienteIdDeSesion();
            if (clienteId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            var viewModel = new ActualizarPerfilViewModel
            {
                Nombre = cliente.Nombre,
                Cedula = cliente.Cedula,
                FechaNacimiento = DateTime.Parse(cliente.FechaNacimiento),
                Telefono = cliente.Telefono,
                CorreoElectronico = cliente.CorreoElectronico
            };

            return View(viewModel);
        }

        // POST: Perfil/Editar
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(ActualizarPerfilViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            // Obtener el ID del usuario de la sesión
            int clienteId = ObtenerClienteIdDeSesion();
            if (clienteId == 0)
            {
                return RedirectToAction("Login", "Account");
            }

            var cliente = await _context.Clientes.FindAsync(clienteId);
            if (cliente == null)
            {
                return NotFound();
            }

            // Verificar la contraseña actual si se ha proporcionado una nueva
            if (!string.IsNullOrEmpty(viewModel.NuevaContrasena))
            {
                if (string.IsNullOrEmpty(viewModel.ContrasenaActual) ||
                    cliente.Contrasena != viewModel.ContrasenaActual)
                {
                    ModelState.AddModelError("ContrasenaActual", "La contraseña actual es incorrecta");
                    return View(viewModel);
                }

                // Actualizar la contraseña
                cliente.Contrasena = viewModel.NuevaContrasena;
            }

            // Actualizar los demás campos
            cliente.Nombre = viewModel.Nombre;
            cliente.Cedula = viewModel.Cedula;
            cliente.FechaNacimiento = viewModel.FechaNacimiento.ToString("yyyy-MM-dd");
            cliente.Telefono = viewModel.Telefono;
            cliente.CorreoElectronico = viewModel.CorreoElectronico;

            try
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Perfil actualizado correctamente";
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "No se pudo guardar los cambios. Intente nuevamente.");
                return View(viewModel);
            }
        }

        // Método auxiliar para obtener el ID del cliente desde la sesión
        private int ObtenerClienteIdDeSesion()
        {
            // Aquí implementaría el código para obtener el ID del cliente desde la sesión
            // Por ahora, devolvemos un valor simulado para testing
            // En un entorno real, esto se obtendría de HttpContext.Session o User.Claims
            return HttpContext.Session.GetInt32("ClienteId") ?? 0;
        }
    }
}