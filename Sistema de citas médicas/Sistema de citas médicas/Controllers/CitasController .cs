using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas_.Data;
using Sistema_de_citas_médicas_.Models;

namespace Sistema_de_citas_médicas_.Controllers
{
    public class CitasController : Controller
    {
        private readonly CitaMedicaContext _context;

        public CitasController(CitaMedicaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Obtener todos los pacientes con sus citas relacionadas
            var pacientes = _context.Pacientes.Include(c => c.Citas).ToList();

            // Crear una lista de ViewModels
            var pacientesCitas = new List<PacienteCitasViewModel>();
            foreach (var paciente in pacientes)
            {
                pacientesCitas.Add(new PacienteCitasViewModel
                {
                    Paciente = paciente,
                    Citas = paciente.Citas
                });
            }

            // Pasar la lista de ViewModels a la vista
            return View(pacientesCitas);
        }
    }
}
