using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsF.Models
{
    [Table("roles_t")]
    public class roles_t
    {
        [Key]
        [Column("id_roles")]  
        public int id_roles { get; set; }

        [Column("nombre")]
        public string nombre { get; set; }
    }
}
