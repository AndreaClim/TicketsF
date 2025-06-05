using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class roles_t
    {
        [Key]
        public int id_roles { get; set; }
        public string nombre { get; set; }
    }
}
