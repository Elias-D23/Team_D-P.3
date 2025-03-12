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
            // Obtener todos los clientes con sus citas relacionadas
            var clientes = _context.Clientes.Include(c => c.Citas).ToList();

            // Crear una lista de ViewModels
            var clientesCitas = new List<ClienteCitasViewModel>();
            foreach (var cliente in clientes)
            {
                clientesCitas.Add(new ClienteCitasViewModel
                {
                    Cliente = cliente,
                    Citas = cliente.Citas
                });
            }

            // Pasar la lista de ViewModels a la vista
            return View(clientesCitas);
        }
    }
}
