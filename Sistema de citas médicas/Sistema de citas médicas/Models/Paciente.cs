namespace Sistema_de_citas_médicas_.Models
{
    public class Paciente
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; } // Considera encriptar esto en una aplicación real
        public List<Cita> Citas { get; set; } = new List<Cita>(); // Relación 1 a muchos
    }
}
