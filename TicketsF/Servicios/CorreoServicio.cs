using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using TicketsF.Servicios;

public class CorreoServicio : ICorreoServicio
{
    private readonly IConfiguration _config;

    public CorreoServicio(IConfiguration config)
    {
        _config = config;
    }

    public void EnviarCorreo(string destinatario, string asunto, string cuerpo, string remitenteTipo = "Admin")
    {
        var remitente = _config[$"Correos:{remitenteTipo}:Remitente"];
        var clave = _config[$"Correos:{remitenteTipo}:Clave"];

        if (string.IsNullOrEmpty(remitente) || string.IsNullOrEmpty(clave))
            throw new Exception("Remitente o clave no configurados correctamente.");

        var mail = new MailMessage
        {
            From = new MailAddress(remitente),
            Subject = asunto,
            Body = cuerpo,
            IsBodyHtml = true
        };
        mail.To.Add(destinatario);

        var smtp = new SmtpClient("smtp.gmail.com")
        {
            Port = 587,
            Credentials = new NetworkCredential(remitente, clave),
            EnableSsl = true
        };

        smtp.Send(mail);
    }
}
