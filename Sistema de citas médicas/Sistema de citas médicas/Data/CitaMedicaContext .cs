using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas_.Models;

namespace Sistema_de_citas_médicas_.Data
{
    public class CitaMedicaContext : DbContext
    {
            public DbSet<Paciente> Pacientes { get; set; }
            public DbSet<Cita> Citas { get; set; }

            public CitaMedicaContext(DbContextOptions<CitaMedicaContext> options) : base(options) { } // Constructor que recibe DbContextOptions

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                // Configurar la relación unidireccional (opcional, ya que EF Core la detecta automáticamente)
                modelBuilder.Entity<Paciente>()
                    .HasMany(c => c.Citas)
                    .WithOne()
                    .HasForeignKey(c => c.PacienteId);

                // Configuraciones adicionales (si es necesario)
            }
        
    }
}
