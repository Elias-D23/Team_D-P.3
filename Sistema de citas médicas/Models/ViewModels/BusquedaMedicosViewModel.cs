using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema_de_citas_médicas.Models.ViewModels
{
    public class BusquedaMedicosViewModel
    {
        // Criterios de búsqueda
        [Display(Name = "Nombre del médico")]
        public string NombreMedico { get; set; }

        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; }

        [Display(Name = "Fecha disponible")]
        [DataType(DataType.Date)]
        public DateTime? FechaDisponible { get; set; }

        // Resultados de búsqueda
        public List<MedicoResultadoViewModel> Resultados { get; set; } = new List<MedicoResultadoViewModel>();

        // Lista de especialidades para el dropdown
        public List<string> Especialidades { get; set; } = new List<string>();
    }

    public class MedicoResultadoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Especialidad { get; set; }
        public string Descripcion { get; set; }
        public int AnosExperiencia { get; set; }
        public string UrlFoto { get; set; }
        public List<string> HorariosDisponibles { get; set; } = new List<string>();
    }
}