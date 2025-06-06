namespace TicketsF.Models
{
    public class DashboardData
    {
        public int TotalTickets { get; set; }
        public int TicketsAbiertos { get; set; }
        public int TicketsEnProgreso { get; set; }
        public int TicketsResueltos { get; set; }
        public int TicketsCerrados { get; set; }

        public int TicketsEnEspera { get; set; }
        public List<usuarios> Clientes { get; set; }
        public List<usuarios> Usuarios { get; set; }
        public List<tickets> Tickets { get; set; }
        public List<prioridad> Prioridades { get; set; } 
        public List<estado> Estado { get; set; }
    }
}
