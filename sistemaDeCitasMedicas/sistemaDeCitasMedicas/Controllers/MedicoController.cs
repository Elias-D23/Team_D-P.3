using Microsoft.AspNetCore.Mvc;
using sistemaDeCitasMedicas.Data;
using Microsoft.EntityFrameworkCore;
using sistemaDeCitasMedicas.Models;
using System.Linq;
using System.Threading.Tasks;

namespace sistemaDeCitasMedicas.Controllers
{
    public class MedicoController : Controller
    {
        private readonly CitaMedicaContext _context;

        public MedicoController(CitaMedicaContext context)
        {
            _context = context;
        }

        // Acción para mostrar los médicos, con filtros de especialidad y tanda
        public async Task<IActionResult> Buscar(string especialidad, string tanda)
        {
            var query = _context.Medicos
                .Include(m => m.MedicoDisponibilidades)
                .ThenInclude(md => md.Disponibilidad)
                .AsQueryable();

            if (!string.IsNullOrEmpty(especialidad))
            {
                query = query.Where(m => m.Especialidad.Contains(especialidad));
            }

            if (!string.IsNullOrEmpty(tanda))
            {
                query = query.Where(m => m.MedicoDisponibilidades
                                         .Any(md => md.Disponibilidad.Tanda.Contains(tanda)));
            }

            var medicos = await query.ToListAsync();
            var especialidades = await _context.Medicos.Select(m => m.Especialidad).Distinct().ToListAsync();
            var tandas = new[] { "Mañana", "Tarde", "Noche" };

            ViewBag.Especialidades = especialidades;
            ViewBag.Tandas = tandas;

            return View(medicos);
        }


        // Acción para buscar médicos con nombre, especialidad y disponibilidad
        public async Task<IActionResult> Buscar(string nombreMedico, string especialidad, string tanda)
        {
            var query = _context.Medicos
                .Include(m => m.MedicoDisponibilidades)
                .ThenInclude(md => md.Disponibilidad)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombreMedico))
            {
                query = query.Where(m => m.Nombre.Contains(nombreMedico));
            }

            if (!string.IsNullOrEmpty(especialidad))
            {
                query = query.Where(m => m.Especialidad == especialidad);
            }

            if (!string.IsNullOrEmpty(tanda))
            {
                query = query.Where(m => m.MedicoDisponibilidades
                                         .Any(md => md.Disponibilidad.Tanda == tanda));
            }

            var medicos = await query.ToListAsync();
            return View(medicos);
        }

        // Detalle de un médico específico
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .Include(m => m.MedicoDisponibilidades)
                .ThenInclude(md => md.Disponibilidad)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }
    }
}
