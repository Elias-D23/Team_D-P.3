using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Sistema_de_citas_médicas.Models
{
    public class Medico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "La especialidad es obligatoria")]
        public string Especialidad { get; set; }

        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Años de experiencia")]
        public int AnosExperiencia { get; set; }

        [Display(Name = "Número de licencia")]
        public string NumeroLicencia { get; set; }

        [Display(Name = "Disponible")]
        public bool Disponible { get; set; }

        [Display(Name = "Foto")]
        public string UrlFoto { get; set; }

        // Horarios disponibles del médico (podría ser una relación con otra tabla)
        public virtual ICollection<Horario> Horarios { get; set; }
    }

    public class Horario
    {
        public int Id { get; set; }
        public int MedicoId { get; set; }
        public DayOfWeek DiaSemana { get; set; }
        public TimeSpan HoraInicio { get; set; }
        public TimeSpan HoraFin { get; set; }
        public virtual Medico Medico { get; set; }
    }
}