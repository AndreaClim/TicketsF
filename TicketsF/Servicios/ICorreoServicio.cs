namespace TicketsF.Servicios
{
    public interface ICorreoServicio
    {
        void EnviarCorreo(string destinatario, string asunto, string cuerpo, string remitenteTipo = "Admin");
    }

}
