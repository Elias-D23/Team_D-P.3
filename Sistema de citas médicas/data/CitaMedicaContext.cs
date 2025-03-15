using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sistema_de_citas_médicas.Models;

namespace Sistema_de_citas_médicas.Data
{
    public class CitaMedicaContext : DbContext
    {
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Horario> Horarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Horario>()
                .HasOne(h => h.Medico)
                .WithMany(m => m.Horarios)
                .HasForeignKey(h => h.MedicoId);
        }

        public async Task InicializarDatosMedicosPrueba()
        {
            if (await Medicos.AnyAsync())
            {
                return;
            }

            var especialidades = new[]
            {
                "Cardiología", "Dermatología", "Gastroenterología",
                "Neurología", "Pediatría", "Oftalmología",
                "Ortopedia", "Ginecología", "Urología"
            };

            var medicos = new List<Medico>();
            var random = new Random();

            for (int i = 1; i <= 15; i++)
            {
                var especialidadIndex = random.Next(0, especialidades.Length);
                var experiencia = random.Next(1, 30);
                var esDisponible = random.Next(0, 10) < 8;

                var medico = new Medico
                {
                    Nombre = $"Dr. {(random.Next(0, 2) == 0 ? "Juan" : "María")} {(char)('A' + random.Next(0, 26))}.{(char)('A' + random.Next(0, 26))}",
                    Especialidad = especialidades[especialidadIndex],
                    Descripcion = $"Especialista en {especialidades[especialidadIndex]} con {experiencia} años de experiencia.",
                    AnosExperiencia = experiencia,
                    NumeroLicencia = $"LIC-{random.Next(1000, 9999)}",
                    Disponible = esDisponible,
                    UrlFoto = $"/img/doctor-{random.Next(1, 10)}.jpg",
                    Horarios = new List<Horario>()
                };

                int numHorarios = random.Next(3, 6);
                for (int j = 0; j < numHorarios; j++)
                {
                    var diaSemana = (DayOfWeek)random.Next(1, 6);
                    var horaInicio = new TimeSpan(8 + random.Next(0, 8), 0, 0);
                    var horaFin = horaInicio.Add(new TimeSpan(2, 0, 0));

                    medico.Horarios.Add(new Horario
                    {
                        DiaSemana = diaSemana,
                        HoraInicio = horaInicio,
                        HoraFin = horaFin,
                        Medico = medico
                    });
                }

                medicos.Add(medico);
            }

            await Medicos.AddRangeAsync(medicos);
            await SaveChangesAsync();
        }
    }
}