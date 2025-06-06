using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace TicketsF.Models
{
    public class notificaciones
    {
        [Key]
        public int id_notif { get; set; }

        public string mensaje { get; set; }

        public DateTime fecha_envio { get; set; }

        public int id_ticket { get; set; }

        public int id_usuarios { get; set; }

        [ForeignKey(nameof(id_ticket))]
        public tickets Ticket { get; set; }

        [ForeignKey(nameof(id_usuarios))]
        public usuarios Usuario { get; set; }
    }
}
