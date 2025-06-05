using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class notificaciones
    {
        [Key]
        public int id_notif { get; set; }
        public string mensaje { get; set; }
        public DateTime fecha_envio { get; set; }
        public int id_usuario { get; set; }
        public usuarios usuario { get; set; }
        public int id_ticket { get; set; }
        public tickets ticket { get; set; }
    }
}
