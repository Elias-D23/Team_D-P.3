using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace sistemaDeCitasMedicas.Services
{
    public class CorreoService
    {
        private readonly string _emailRemitente = "frefroigoicaki-7188@yopmail.com";
        private readonly string _smtpServer = "sandbox.smtp.mailtrap.io";
        private readonly int _smtpPort = 587;
        private readonly string _smtpUsername = "f5857a92c521e4";
        private readonly string _smtpPassword = "1a7f7dc9c98034";
        private readonly string _urlConfirmacion = "http://localhost:5000/confirmar";

        public void EnviarCorreoConfirmacion(string correo, string token)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Sistema de Citas", _emailRemitente));
            mensaje.To.Add(new MailboxAddress("", correo));
            mensaje.Subject = "Confirma tu correo";
            mensaje.Body = new TextPart("plain")
            {
                Text = $"Haz clic en este enlace para confirmar: {_urlConfirmacion}?token={token}"
            };

            try
            {
                using (var smtp = new SmtpClient())
                {
                    smtp.Connect(_smtpServer, _smtpPort, false);
                    smtp.Authenticate(_smtpUsername, _smtpPassword);
                    smtp.Send(mensaje);
                    smtp.Disconnect(true);
                    Console.WriteLine("Correo enviado exitosamente");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar el correo: {ex.Message}");
                throw new Exception("Error al enviar el correo de confirmación.");
            }
        }
    }
}
