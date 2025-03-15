using System.ComponentModel.DataAnnotations;

namespace sistemaDeCitasMedicas.Models
{
    public class Medico
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Especialidad { get; set; }

        [Required]
        public string HorarioDisponible { get; set; } // "Mañana, Tarde, Noche"
    }
}
