using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace sistemaDeCitasMedicas.Services
{
    public class CorreoService
    {
        // Datos de configuración dentro de la clase
        private readonly string _emailRemitente = "no-reply@hospital.com";  // Correo del remitente
        private readonly string _smtpServer = "sandbox.smtp.mailtrap.io"; // Servidor SMTP de Mailtrap
        private readonly int _smtpPort = 587;  // Puerto SMTP para TLS
        private readonly string _smtpUsername = "2fa3dde51fc932";  // Usuario de Mailtrap
        private readonly string _smtpPassword = "832ccafe3237a8";  // Contraseña de Mailtrap
        private readonly string _urlConfirmacion = "http://localhost:5000/confirmar";  // URL de confirmación (puedes cambiarla si es necesario)

        // Método para enviar el correo de confirmación
        public void EnviarCorreoConfirmacion(string correo, string token)
        {
            // Crear el mensaje MIME
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Sistema de Citas", _emailRemitente));  // Remitente
            mensaje.To.Add(new MailboxAddress("", correo));  // Destinatario
            mensaje.Subject = "Confirma tu correo";  // Asunto del correo
            mensaje.Body = new TextPart("plain")  // Cuerpo del correo
            {
                Text = $"Haz clic en este enlace para confirmar: {_urlConfirmacion}?token={token}"
            };

            // Configurar y enviar el correo a través del servidor SMTP
            using (var smtp = new SmtpClient())
            {
                smtp.Connect(_smtpServer, _smtpPort, false);  // Conectar al servidor SMTP
                smtp.Authenticate(_smtpUsername, _smtpPassword);  // Autenticación SMTP
                smtp.Send(mensaje);  // Enviar el mensaje
                smtp.Disconnect(true);  // Desconectar
            }
        }
    }
}
