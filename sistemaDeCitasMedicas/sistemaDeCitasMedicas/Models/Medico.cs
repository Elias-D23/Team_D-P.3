using System.ComponentModel.DataAnnotations;

namespace sistemaDeCitasMedicas.Models
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

        // Relación con las disponibilidades
        public virtual ICollection<MedicoDisponibilidad> MedicoDisponibilidades { get; set; }
    }

    public class Disponibilidad
    {
        public int Id { get; set; }

        [Required]
        public DayOfWeek DiaSemana { get; set; } // Lunes, Martes, etc.

        [Required]
        public string Tanda { get; set; } // Mañana, Tarde, Noche

        // Relación con los médicos
        public virtual ICollection<MedicoDisponibilidad> MedicoDisponibilidades { get; set; }
    }

    public class MedicoDisponibilidad
    {
        public int MedicoId { get; set; }
        public Medico Medico { get; set; }

        public int DisponibilidadId { get; set; }
        public Disponibilidad Disponibilidad { get; set; }
    }
}