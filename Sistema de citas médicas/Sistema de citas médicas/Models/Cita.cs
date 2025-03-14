namespace Sistema_de_citas_médicas_.Models
{
    public class Cita
    {

        public int id { get; set; }
        public int PacienteId { get; set; } // Clave foránea
        public DateTime FechaHora { get; set; }
        public string Medico { get; set; }
        public string Especialidad { get; set; }
        public string RequisitosEspeciales { get; set; }
    }
}