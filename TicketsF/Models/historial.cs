using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class historial
    {
        [Key]
        public int id_historial { get; set; }
        public string mensaje { get; set; }
        public DateTime fecha_cambio { get; set; }
        public int id_ticket { get; set; }
        public tickets ticket { get; set; }
        public int id_estadoA { get; set; }
        public estado estadoA { get; set; }
        public int id_estadoN { get; set; }
        public estado estadoN { get; set; }
    }
}
