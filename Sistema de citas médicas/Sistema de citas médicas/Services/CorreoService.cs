using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace Sistema_de_citas_médicas_.Services
{
    public class CorreoService
    {
        private readonly string _emailRemitente = "citasmedicas@hospital.com";
        private readonly string _password = "tucontraseña";

        public void EnviarCorreoConfirmacion(string correo, string token)
        {
            var mensaje = new MimeMessage();
            mensaje.From.Add(new MailboxAddress("Sistema de Citas", _emailRemitente));
            mensaje.To.Add(new MailboxAddress("", correo));
            mensaje.Subject = "Confirma tu correo";
            mensaje.Body = new TextPart("plain")
            {
                Text = $"Haz clic en este enlace para confirmar: http://tusitio.com/confirmar?token={token}"
            };

            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 587, false);
                smtp.Authenticate(_emailRemitente, _password);
                smtp.Send(mensaje);
                smtp.Disconnect(true);
            }
        }
    }
}
