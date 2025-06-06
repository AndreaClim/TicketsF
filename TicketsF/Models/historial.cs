using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsF.Models
{
    public class historial
    {
        [Key]
        public int id_historial { get; set; }

        public string mensaje { get; set; }

        public DateTime fecha_cambio { get; set; }

        public int id_ticket { get; set; }
        public int id_estadoA { get; set; }
        public int id_estadoN { get; set; }

        [ForeignKey(nameof(id_ticket))]
        public tickets Ticket { get; set; }

        [ForeignKey(nameof(id_estadoA))]
        public estado EstadoAnterior { get; set; }

        [ForeignKey(nameof(id_estadoN))]
        public estado EstadoNuevo { get; set; }
    }
}
