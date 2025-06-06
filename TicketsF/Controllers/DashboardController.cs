using Microsoft.AspNetCore.Mvc;
using TicketsF.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            var totalTickets = _context.tickets.Count();
            var ticketsAbiertos = _context.tickets.Where(t => t.id_estado == 1).Count();
            var ticketsEnProgreso = _context.tickets.Where(t => t.id_estado == 2).Count();
            var ticketsResueltos = _context.tickets.Where(t => t.id_estado == 4).Count();

            var clientes = _context.usuarios.Where(u => u.roles == "Cliente").ToList();
            var usuarios = _context.usuarios.ToList();
            var tickets = _context.tickets
                .Include(t => t.usuarioC)
                .Include(t => t.usuarioE)
                .Include(t => t.estado)
                .Include(t => t.prioridad)
                .Include(t => t.categoria)
                .ToList();

            var prioridades = _context.prioridad.ToList(); 
            var estados = _context.estado.ToList(); 
            var dashboardData = new DashboardData
            {
                TotalTickets = totalTickets,
                TicketsAbiertos = ticketsAbiertos,
                TicketsEnProgreso = ticketsEnProgreso,
                TicketsResueltos = ticketsResueltos,
                Clientes = clientes,
                Usuarios = usuarios,
                Tickets = tickets,
                Prioridades = prioridades,
                Estado = estados 
            };
            return View(dashboardData);



            return View(dashboardData);
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

  


        public IActionResult ResolverTicket(int id, int idUsuarioAsignado, int idPrioridad, int idEstado)
        {
            // Buscar el ticket a resolver
            var ticket = _context.tickets
                .Include(t => t.usuarioE)
                .Include(t => t.prioridad)
                .FirstOrDefault(t => t.id_ticket == id);

            if (ticket != null)
            {
                // Actualizar la información del ticket
                ticket.id_usuarioE = idUsuarioAsignado;
                ticket.id_prioridad = idPrioridad;
                ticket.id_estado = idEstado; // Cambiar el estado a 'En Progreso' o cualquier otro
                _context.SaveChanges();
            }

            // Obtener los datos actualizados del Dashboard
            var totalTickets = _context.tickets.Count();
            var ticketsAbiertos = _context.tickets.Where(t => t.id_estado == 1).Count();
            var ticketsEnProgreso = _context.tickets.Where(t => t.id_estado == 2).Count();
            var ticketsResueltos = _context.tickets.Where(t => t.id_estado == 4).Count();

            var clientes = _context.usuarios.Where(u => u.roles == "Cliente").ToList();
            var usuarios = _context.usuarios.ToList();
            var tickets = _context.tickets
                .Include(t => t.usuarioC)
                .Include(t => t.usuarioE)
                .Include(t => t.estado)
                .Include(t => t.prioridad)
                .Include(t => t.categoria)
                .ToList();

            var prioridades = _context.prioridad.ToList();
            var estados = _context.estado.ToList();

            var dashboardData = new DashboardData
            {
                TotalTickets = totalTickets,
                TicketsAbiertos = ticketsAbiertos,
                TicketsEnProgreso = ticketsEnProgreso,
                TicketsResueltos = ticketsResueltos,
                Clientes = clientes,
                Usuarios = usuarios,
                Tickets = tickets,
                Prioridades = prioridades,
                Estado = estados
            };

            // Devolver solo el fragmento de la vista del Dashboard (sin recargar toda la página)
            return PartialView("_Dashboard", dashboardData);
        }












    }
}
