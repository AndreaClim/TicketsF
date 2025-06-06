using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace TicketsF.Models
{
    public class notificaciones
    {
        [Key]
        public int id_notif { get; set; }

        public int id_usuarios { get; set; }
        public int? id_ticket { get; set; }  
        public string mensaje { get; set; }
        public DateTime fecha_envio { get; set; }

        [ForeignKey("id_usuarios")]
        public virtual usuarios usuario { get; set; }

        [ForeignKey("id_ticket")]
        public virtual tickets ticket { get; set; }
    }
}
