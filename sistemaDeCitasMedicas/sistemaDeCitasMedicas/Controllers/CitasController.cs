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


        [HttpPost]
        public IActionResult Eliminar(int id)
        {
            var paciente = _context.Pacientes.Include(p => p.Citas).FirstOrDefault(p => p.id == id);

            if (paciente == null)
            {
                return NotFound();
            }

            // Eliminar todas las citas asociadas antes de eliminar el paciente
            _context.Citas.RemoveRange(paciente.Citas);
            _context.Pacientes.Remove(paciente);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Info(int id)
        {
            var paciente = _context.Pacientes
                .Include(p => p.Citas)
                .FirstOrDefault(p => p.id == id);

            if (paciente == null)
            {
                return NotFound();
            }

            var viewModel = new PacienteCitasViewModel
            {
                Paciente = paciente,
                Citas = paciente.Citas
            };

            return View(viewModel);
        }

        // Acción para mostrar la vista de edición del paciente
        public IActionResult EditarPaciente(int id)
        {
            var paciente = _context.Pacientes.FirstOrDefault(p => p.id == id);
            if (paciente == null)
            {
                return NotFound();
            }

            return View(paciente);
        }

        // Acción para guardar los cambios del paciente
        [HttpPost]
        public IActionResult EditarPaciente(Paciente pacienteEditado)
        {
            if (ModelState.IsValid)
            {
                // Buscar el paciente en la base de datos
                var paciente = _context.Pacientes.FirstOrDefault(p => p.id == pacienteEditado.id);

                if (paciente == null)
                {
                    return NotFound();
                }

                // Actualizar solo los campos del paciente
                paciente.Nombre = pacienteEditado.Nombre;
                paciente.Cedula = pacienteEditado.Cedula;
                paciente.FechaNacimiento = pacienteEditado.FechaNacimiento;
                paciente.Telefono = pacienteEditado.Telefono;
                paciente.CorreoElectronico = pacienteEditado.CorreoElectronico;
                paciente.Contrasena = pacienteEditado.Contrasena; // Considera encriptar la contraseña

                // Guardar los cambios
                _context.Pacientes.Update(paciente);
                _context.SaveChanges();

                TempData["Mensaje"] = "Paciente editado correctamente.";


                return RedirectToAction("Index");
            }

            return View(pacienteEditado);
        }


        [HttpPost]
        public IActionResult EliminarPaciente(int id)
        {
            var paciente = _context.Pacientes.Include(p => p.Citas).FirstOrDefault(p => p.id == id);

            if (paciente == null)
            {
                return NotFound();
            }

            // Eliminar todas las citas asociadas antes de eliminar el paciente
            _context.Citas.RemoveRange(paciente.Citas);
            _context.Pacientes.Remove(paciente);
            _context.SaveChanges();

            // Agregar mensaje de éxito a TempData
            TempData["Mensaje"] = "Paciente eliminado correctamente.";

            return RedirectToAction("Index");
        }



        //Citas -------------------------------------

        [HttpPost]
        public IActionResult EliminarCita(int id, int pacienteId)
        {
            var cita = _context.Citas.FirstOrDefault(c => c.id == id);

            if (cita != null)
            {
                _context.Citas.Remove(cita);
                _context.SaveChanges();
            }

            return RedirectToAction("Info", new { id = pacienteId });
        }

        public IActionResult AgregarCita(int pacienteId)
        {
            var paciente = _context.Pacientes.FirstOrDefault(p => p.id == pacienteId);
            if (paciente == null)
            {
                return NotFound();
            }

            var cita = new Cita { PacienteId = pacienteId };
            return View(cita);
        }


        [HttpPost]
        public IActionResult AgregarCita(Cita nuevaCita)
        {
            if (ModelState.IsValid)
            {
                _context.Citas.Add(nuevaCita);
                _context.SaveChanges();
                return RedirectToAction("Info", new { id = nuevaCita.PacienteId });
            }
            return View(nuevaCita);
        }


        // Cargar la vista de edición con la cita existente
        public IActionResult EditarCita(int id)
        {
            var cita = _context.Citas.FirstOrDefault(c => c.id == id);
            if (cita == null)
            {
                return NotFound();
            }
            return View(cita);
        }

        // Guardar cambios en la cita
        [HttpPost]
        public IActionResult EditarCita(Cita citaEditada)
        {
            if (ModelState.IsValid)
            {
                _context.Citas.Update(citaEditada);
                _context.SaveChanges();
                return RedirectToAction("Info", new { id = citaEditada.PacienteId });
            }
            return View(citaEditada);
        }


    }
}
