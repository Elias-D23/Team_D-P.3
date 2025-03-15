using Microsoft.AspNetCore.Mvc;
using sistemaDeCitasMedicas.Data;
using Microsoft.EntityFrameworkCore; //
using sistemaDeCitasMedicas.Models;
using System.Linq; //
using System.Threading.Tasks; //

namespace sistemaDeCitasMedicas.Controllers
{
    public class MedicoController : Controller
    {
        private readonly CitaMedicaContext _context;

        public MedicoController(CitaMedicaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string especialidad, string horario)
        {
            var medicos = _context.Medicos.AsQueryable();

            if (!string.IsNullOrEmpty(especialidad))
            {
                medicos = medicos.Where(m => m.Especialidad == especialidad);
            }

            if (!string.IsNullOrEmpty(horario))
            {
                medicos = medicos.Where(m => m.HorarioDisponible.Contains(horario));
            }

            var especialidades = await _context.Medicos.Select(m => m.Especialidad).Distinct().ToListAsync();
            var horarios = new[] { "Mañana", "Tarde", "Noche" };

            ViewBag.Especialidades = especialidades;
            ViewBag.Horarios = horarios;

            return View(await medicos.ToListAsync());
        }
    }
}
