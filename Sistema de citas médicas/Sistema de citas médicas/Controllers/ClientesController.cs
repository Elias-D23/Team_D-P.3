using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas_.Data;
using Sistema_de_citas_médicas_.Models;
using System.Threading.Tasks;

namespace Sistema_de_citas_médicas_.Controllers
{
    public class ClientesController : Controller
    {
        private readonly CitaMedicaContext _context;

        public ClientesController(CitaMedicaContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes.Include(c => c.Citas).ToListAsync();
            return View(clientes);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .Include(c => c.Citas)
                .FirstOrDefaultAsync(m => m.id == id);

            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);

            if (cliente == null)
            {
                return NotFound();
            }

            // Primero, eliminar todas las citas asociadas al cliente
            var citasCliente = await _context.Citas.Where(c => c.ClienteId == id).ToListAsync();
            _context.Citas.RemoveRange(citasCliente);

            // Luego, eliminar el cliente
            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();

            TempData["Message"] = "El paciente ha sido eliminado correctamente.";
            return RedirectToAction(nameof(Index), "Citas");
        }
    }
}