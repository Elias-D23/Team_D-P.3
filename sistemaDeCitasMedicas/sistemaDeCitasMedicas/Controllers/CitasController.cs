using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sistemaDeCitasMedicas.Data;
using sistemaDeCitasMedicas.Models;

namespace sistemaDeCitasMedicas.Controllers
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


        public IActionResult Crear()
        {
            return View(new PacienteCitasViewModel()); // Se pasa un ViewModel vacío para poblar la vista
        }

        [HttpPost]
        public IActionResult Crear(PacienteCitasViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Crear nuevo paciente
                var paciente = new Paciente
                {
                    Nombre = model.Paciente.Nombre,
                    Cedula = model.Paciente.Cedula,
                    FechaNacimiento = model.Paciente.FechaNacimiento,
                    Telefono = model.Paciente.Telefono,
                    CorreoElectronico = model.Paciente.CorreoElectronico,
                    Contrasena = model.Paciente.Contrasena, // Considera encriptar
                    Citas = new List<Cita>() // Inicializamos la lista
                };

                _context.Pacientes.Add(paciente);
                _context.SaveChanges(); // Se guarda primero el paciente para obtener su id

                // Agregar la cita si existe en el formulario
                if (model.Citas != null && model.Citas.Any())
                {
                    foreach (var cita in model.Citas)
                    {
                        cita.PacienteId = paciente.id; // Asignamos el ID del paciente
                        _context.Citas.Add(cita);
                    }
                    _context.SaveChanges(); // Guardamos las citas después de asignar el ID del paciente
                }

                return RedirectToAction("Index");
            }

            return View(model); // Retorna la vista en caso de error de validación
        }
    }
}
