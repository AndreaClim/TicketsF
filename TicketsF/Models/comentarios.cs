using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;

namespace TicketsF.Models
{
    public class comentarios
    {
        [Key]
        public int id_comentario { get; set; }
        public string comentario { get; set; }
        public DateTime fecha { get; set; }

        [ForeignKey("ticket")]
        public int id_ticket { get; set; }

        [ForeignKey("usuario")]
        public int id_usuarios { get; set; }
        // Relaciones
        public tickets ticket { get; set; }
        public usuarios usuario { get; set; }
    }
}
