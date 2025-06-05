using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class estado
    {
        [Key]
        public int id_estado { get; set; }
        public string nombre { get; set; }
      
    }
}
