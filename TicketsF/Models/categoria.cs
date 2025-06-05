using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class categoria
    {
        [Key]
        public int id_cat { get; set; }
        public string nombre { get; set; }
    }
}
