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

            // Obtener todos los usuarios y filtrar clientes y empleados
            var clientes = _context.usuarios.Where(u => u.roles == "Cliente").ToList();
            var usuarios = _context.usuarios.Where(u => u.roles != "Cliente").ToList(); // Todos los empleados

            // Crear el objeto DashboardData
            var dashboardData = new DashboardData
            {
                TotalTickets = totalTickets,
                TicketsAbiertos = ticketsAbiertos,
                TicketsEnProgreso = ticketsEnProgreso,
                TicketsResueltos = ticketsResueltos,
                Clientes = clientes,
                Usuarios = usuarios
            };

            return View(dashboardData); // Pasar la instancia de DashboardData a la vista
        }
        public IActionResult Eliminar(int id)
        {
            var usuario = _context.usuarios.Find(id);
            if (usuario != null)
            {
                _context.usuarios.Remove(usuario);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult EliminarC(int id)
        {
            var cliente = _context.usuarios.Find(id);
            if (cliente != null)
            {
                _context.usuarios.Remove(cliente);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Dashboard");
        }


    }
}
