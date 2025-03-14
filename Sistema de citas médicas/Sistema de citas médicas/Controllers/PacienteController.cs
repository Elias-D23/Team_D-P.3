using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using Sistema_de_citas_médicas_.Data;
using Sistema_de_citas_médicas_.Models;
using Sistema_de_citas_médicas_.Services;
using System;

namespace Sistema_de_citas_médicas_.Controllers
{
    public class PacienteController : Controller
    {
        private readonly CorreoService _correoService;

        public PacienteController(CorreoService correoService)
        {
            _correoService = correoService;
        }

        [HttpPost("registrar")]
        public IActionResult Registrar([FromBody] Paciente paciente)
        {
            if (ModelState.IsValid)
            {
                // Generar token
                var token = TokenService.GenerarTokenValidacion(paciente.CorreoElectronico);

                // Enviar email
                _correoService.EnviarCorreoConfirmacion(paciente.CorreoElectronico, token);

                return Ok("Registro exitoso. Revisa tu correo para confirmar tu cuenta.");
            }

            return BadRequest("Error en los datos.");
        }
    }
}
