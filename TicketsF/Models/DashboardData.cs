namespace TicketsF.Models
{
    public class DashboardData
    {
        public int TotalTickets { get; set; }
        public int TicketsAbiertos { get; set; }
        public int TicketsEnProgreso { get; set; }
        public int TicketsResueltos { get; set; }
        public List<usuarios> Clientes { get; set; }
        public List<usuarios> Usuarios { get; set; }
        public List<tickets> Tickets { get; set; }
    }
}



