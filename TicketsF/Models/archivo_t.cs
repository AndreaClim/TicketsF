using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class archivo_t
    {
        [Key]
        public int id_archivot { get; set; }
        public string link { get; set; }
        public DateTime fecha { get; set; }
        public int id_ticket { get; set; }
        public tickets ticket { get; set; }
    }
}
