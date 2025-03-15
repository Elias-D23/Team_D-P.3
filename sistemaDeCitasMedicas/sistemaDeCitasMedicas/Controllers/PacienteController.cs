using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using NuGet.Common;
using Microsoft.EntityFrameworkCore;
using sistemaDeCitasMedicas.Data;
using sistemaDeCitasMedicas.Models;
using sistemaDeCitasMedicas.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace sistemaDeCitasMedicas.Controllers
{
    public class PacienteController : Controller
    {
        private readonly CitaMedicaContext _context;
        private readonly CorreoService _correoService;

        public PacienteController(CitaMedicaContext context, CorreoService correoService)
        {
            _context = context;
            _correoService = correoService;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Paciente paciente)
        {
            var user = await _context.Pacientes
                .FirstOrDefaultAsync(u => u.CorreoElectronico == paciente.CorreoElectronico && u.Contrasena == paciente.Contrasena);

            if (user == null)
            {
                ModelState.AddModelError("", "Correo o contraseña incorrectos");
                return View();
            }

            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.CorreoElectronico) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Registrar()
        {
            return View();
        }

        //[HttpPost("registrar")]
        //public async Task<IActionResult> Registrar([FromBody] Paciente paciente)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var token = TokenService.GenerarTokenValidacion(paciente.CorreoElectronico);
        //        _correoService.EnviarCorreoConfirmacion(paciente.CorreoElectronico, token);

        //        _context.Pacientes.Add(paciente);
        //        await _context.SaveChangesAsync();

        //        return Ok("Registro exitoso. Revisa tu correo para confirmar tu cuenta.");
        //    }
        //    return BadRequest("Error en los datos.");
        //}
        [HttpPost]
        public async Task<IActionResult> Registrar(Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                var token = TokenService.GenerarTokenValidacion(paciente.CorreoElectronico);
                _correoService.EnviarCorreoConfirmacion(paciente.CorreoElectronico, token);

                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login"); // Enviar a la página de inicio de sesión
            }
            return View(paciente); // Si hay error, regresar la vista con los datos ingresados
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
