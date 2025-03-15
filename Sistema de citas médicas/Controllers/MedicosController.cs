using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas.Data;
using Sistema_de_citas_médicas.Models;
using Sistema_de_citas_médicas.Models.ViewModels;

namespace Sistema_de_citas_médicas.Controllers
{
    public class MedicosController : Controller
    {
        private readonly CitaMedicaContext _context;

        public MedicosController(CitaMedicaContext context)
        {
            _context = context;
        }

        // GET: Medicos/Buscar
        public async Task<IActionResult> Buscar(BusquedaMedicosViewModel model)
        {
            // Cargar las especialidades para el dropdown
            model.Especialidades = await _context.Medicos
                .Select(m => m.Especialidad)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();

            // Si no hay criterios de búsqueda, solo devolver la vista con el modelo inicializado
            if (string.IsNullOrEmpty(model.NombreMedico) &&
                string.IsNullOrEmpty(model.Especialidad) &&
                !model.FechaDisponible.HasValue)
            {
                return View(model);
            }

            // Preparar la consulta base
            var query = _context.Medicos
                .Include(m => m.Horarios)
                .AsQueryable();

            // Aplicar filtros si existen
            if (!string.IsNullOrEmpty(model.NombreMedico))
            {
                query = query.Where(m => m.Nombre.Contains(model.NombreMedico));
            }

            if (!string.IsNullOrEmpty(model.Especialidad))
            {
                query = query.Where(m => m.Especialidad == model.Especialidad);
            }

            // Filtrar por disponibilidad si se especificó una fecha
            if (model.FechaDisponible.HasValue)
            {
                var dia = model.FechaDisponible.Value.DayOfWeek;
                query = query.Where(m => m.Disponible && m.Horarios.Any(h => h.DiaSemana == dia));
            }

            // Ejecutar la consulta y mapear los resultados
            var medicos = await query.ToListAsync();
            model.Resultados = medicos.Select(m => new MedicoResultadoViewModel
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Especialidad = m.Especialidad,
                Descripcion = m.Descripcion,
                AnosExperiencia = m.AnosExperiencia,
                UrlFoto = string.IsNullOrEmpty(m.UrlFoto) ? "/img/doctor-default.png" : m.UrlFoto,
                HorariosDisponibles = m.Horarios
                    .Select(h => $"{h.DiaSemana}: {h.HoraInicio.ToString(@"hh\:mm")} - {h.HoraFin.ToString(@"hh\:mm")}")
                    .ToList()
            }).ToList();

            return View(model);
        }

        // GET: Medicos/Detalles/5
        public async Task<IActionResult> Detalles(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medico = await _context.Medicos
                .Include(m => m.Horarios)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (medico == null)
            {
                return NotFound();
            }

            return View(medico);
        }
    }
}