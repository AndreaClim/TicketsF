using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class prioridad
    {
        [Key]
        public int id_prioridad { get; set; }
        public string nombre { get; set; }

    }
}
