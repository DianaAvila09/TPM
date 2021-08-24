using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Atk_wsHistProdxDia.Properties;

namespace Atk_wsHistProdxDia
{
    class EnviarCorreo
    {
        public void Enviar(string pMensaje)
        {
            //*---------------Definir instancia de la clase MailMessage. ----------------------*/

            MailMessage email = new MailMessage();
            email.To.Add(new MailAddress(Settings.Default.To));
            email.From = new MailAddress(Settings.Default.From);
            email.Subject = Settings.Default.Subject;
            email.Body = pMensaje;
            email.IsBodyHtml = true;
            email.Priority = MailPriority.High;

            // Definir objeto SmtpClient

            SmtpClient smtp = new SmtpClient();
            smtp.Host = Settings.Default.Smtp;
            smtp.Port = 25;
            smtp.EnableSsl = false;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(Settings.Default.To, "contraseña");

            string output = null;

            try
            {
                smtp.Send(email);
                email.Dispose();
                output = "Corre electrónico fue enviado satisfactoriamente.";
            }
            catch (Exception ex)
            {
                output = "Error enviando correo electrónico: " + ex.Message;
            }
        }
    }
}
