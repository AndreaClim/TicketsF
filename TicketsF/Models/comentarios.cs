namespace TicketsF.Models
{
    public class comentarios
    {
        public int id_comentario { get; set; }
        public string comentario { get; set; }
        public DateTime fecha { get; set; }
        public int id_ticket { get; set; }
        public tickets ticket { get; set; }
        public int id_usuario { get; set; }
        public usuarios usuario { get; set; }

    }
}
