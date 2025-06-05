using System.ComponentModel.DataAnnotations;

namespace TicketsF.Models
{
    public class usuarios
    {
        [Key]
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correo { get; set; }
        public string contrasenia { get; set; }
        public string telefono { get; set; }
        public int id_roles { get; set; }
        public roles_t roles { get; set; }
    }
}
