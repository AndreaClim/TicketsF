using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TicketsF.Models;

namespace TicketsF.Models;
public class tickets
{
    [Key]
    public int id_ticket { get; set; }
    public string titulo { get; set; }
    public string descripcion { get; set; }
    public string nombre_app { get; set; }
    public DateTime fecha_creacion { get; set; }

    public int? id_usuarioC { get; set; } // Usuario creador
    public int? id_usuarioE { get; set; } // Usuario asignado
    public int id_cat { get; set; }
    public int id_estado { get; set; }
    public int id_prioridad { get; set; }

    [ForeignKey("id_usuarioC")]
    public virtual usuarios usuarioC { get; set; } // Usuario creador
    [ForeignKey("id_usuarioE")]
    public virtual usuarios usuarioE { get; set; } // Usuario asignado
    [ForeignKey("id_cat")]
    public virtual categoria categoria { get; set; }
    [ForeignKey("id_estado")]
    public virtual estado estado { get; set; }
    [ForeignKey("id_prioridad")]
    public virtual prioridad prioridad { get; set; }
}
