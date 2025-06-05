using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketsF.Models
{
    public class usuarios
    {
        [Key]
        [Column("id_usuarios")]  // Aseguramos que 'id_usuarios' corresponde con la columna de la base de datos
        public int id_usuario { get; set; }

        [Column("nombre")]
        public string nombre { get; set; }

        [Column("apellido")]
        public string apellido { get; set; }

        [Column("correo")]
        public string correo { get; set; }

        [Column("contrasenia")]
        public string contrasenia { get; set; }

        [Column("telefono")]
        public string telefono { get; set; }

        [Column("autenticacion")]
        public string autenticacion { get; set; }

      
        [Column("roles")]
        public string roles { get; set; }  // 'Administrador', 'Técnico', 'Cliente'
    }
}
