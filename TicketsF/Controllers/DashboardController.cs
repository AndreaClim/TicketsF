using Microsoft.AspNetCore.Mvc;
using TicketsF.Models;
using System.Linq;

namespace TicketsF.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ticketsDbContext _context;

        public DashboardController(ticketsDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar el Dashboard
        public IActionResult Index()
        {
            // Obtener las estadísticas de los tickets desde la base de datos
            var totalTickets = _context.tickets.Count(); // Total de tickets
            var ticketsAbiertos = _context.tickets.Where(t => t.id_estado == 1).Count(); // Tickets abiertos
            var ticketsEnProgreso = _context.tickets.Where(t => t.id_estado == 2).Count(); // Tickets en progreso
            var ticketsResueltos = _context.tickets.Where(t => t.id_estado == 4).Count(); // Tickets resueltos

            // Obtener todos los clientes (opcional)
            var clientes = _context.usuarios.ToList();

            // Preparar los datos para pasar a la vista
            var dashboardData = new
            {
                TotalTickets = totalTickets,
                TicketsAbiertos = ticketsAbiertos,
                TicketsEnProgreso = ticketsEnProgreso,
                TicketsResueltos = ticketsResueltos,
                Clientes = clientes
            };

            return View(dashboardData); // Pasamos los datos al Dashboard
        }
    }
}
