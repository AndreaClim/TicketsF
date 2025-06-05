using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class tickets
    {
        [Key]
        public int id_ticket { get; set; }
        public string titulo { get; set; }
        public string descripcion { get; set; }
        public string nombre_app { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int id_usuarioC { get; set; }
        public usuarios usuarios { get; set; }
        public int id_usuarioE { get; set; }
        public usuarios usuariosE { get; set; }
        public int id_cat { get; set; }
        public categoria categoria { get; set; }
        public int id_estado { get; set; }
        public estado estado { get; set; }
        public int id_prioridad { get; set; }
        public prioridad prioridad { get; set; }
    }
}
