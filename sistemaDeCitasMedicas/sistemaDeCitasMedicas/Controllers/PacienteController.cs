using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaDeCitasMedicas.Data;
using sistemaDeCitasMedicas.Models;
using sistemaDeCitasMedicas.Services;
using System.Security.Claims;
using System.Threading.Tasks;
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

        return RedirectToAction("Index", "Paciente"); // Cambié la redirección para ir al perfil del paciente
    }

    public IActionResult Registrar()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Registrar(Paciente paciente)
    {
        if (ModelState.IsValid)
        {
            try
            {
                // Guardar paciente en la base de datos
                _context.Pacientes.Add(paciente);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Registrado con éxito. Ahora puedes iniciar sesión.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hubo un error al registrar el paciente: {ex.Message}";
                return View(paciente); // Mostrar el formulario con el error
            }
        }
        return View(paciente); // Si hay error en el modelo, regresar la vista con los datos
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }

    // Acción para ver el perfil del paciente (Read)
    public async Task<IActionResult> Index()
    {
        var correo = User.Identity.Name; // Obtener el correo del paciente autenticado
        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CorreoElectronico == correo);

        if (paciente == null)
        {
            return RedirectToAction("Login");
        }

        return View(paciente); // Ver los datos del paciente
    }

    // Acción para editar la información del paciente
    public async Task<IActionResult> Edit()
    {
        var correo = User.Identity.Name; // Obtener el correo del paciente autenticado
        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CorreoElectronico == correo);

        if (paciente == null)
        {
            return RedirectToAction("Login");
        }

        return View(paciente); // Editar la información
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Paciente paciente)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.Pacientes.Update(paciente); // Actualizar paciente
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Datos actualizados con éxito.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Hubo un error al actualizar los datos: {ex.Message}";
                return View(paciente);
            }
        }
        return View(paciente); // Si hay error en el modelo, regresar la vista con los datos
    }

    // Acción para eliminar la cuenta del paciente
    public async Task<IActionResult> Delete()
    {
        var correo = User.Identity.Name; // Obtener el correo del paciente autenticado
        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CorreoElectronico == correo);

        if (paciente == null)
        {
            return RedirectToAction("Login");
        }

        return View(paciente); // Confirmación de eliminación
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed()
    {
        var correo = User.Identity.Name; // Obtener el correo del paciente autenticado
        var paciente = await _context.Pacientes.FirstOrDefaultAsync(p => p.CorreoElectronico == correo);

        if (paciente != null)
        {
            _context.Pacientes.Remove(paciente); // Eliminar paciente
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Cuenta eliminada con éxito.";
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Cerrar sesión
        }

        return RedirectToAction("Login");
    }
}
