using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sistemaDeCitasMedicas.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreateSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Cedula = table.Column<string>(type: "TEXT", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Telefono = table.Column<string>(type: "TEXT", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "TEXT", nullable: false),
                    Contrasena = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Citas",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PacienteId = table.Column<int>(type: "INTEGER", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Medico = table.Column<string>(type: "TEXT", nullable: false),
                    Especialidad = table.Column<string>(type: "TEXT", nullable: false),
                    RequisitosEspeciales = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Citas", x => x.id);
                    table.ForeignKey(
                        name: "FK_Citas_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citas_PacienteId",
                table: "Citas",
                column: "PacienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Citas");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
