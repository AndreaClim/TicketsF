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
            var ticketsAbiertos = _context.tickets.Count(t => t.id_estado == 1);
            var ticketsEnProgreso = _context.tickets.Count(t => t.id_estado == 2);
            var ticketsEnEspera = _context.tickets.Count(t => t.id_estado == 3);
            var ticketsResueltos = _context.tickets.Count(t => t.id_estado == 4);
            var ticketsCerrados = _context.tickets.Count(t => t.id_estado == 5);

            var clientes = _context.usuarios.Where(u => u.roles == "Cliente").ToList();
            var usuarios = _context.usuarios.ToList();
            var tickets = _context.tickets
                .Where(t => t.id_estado != 5) 
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
                TicketsEnEspera = ticketsEnEspera,
                TicketsCerrados = ticketsCerrados,
                Clientes = clientes,
                Usuarios = usuarios,
                Tickets = tickets,
                Prioridades = prioridades,
                Estado = estados
            };

            return View(dashboardData);
        }


        public IActionResult ResolverTicket(int id, int idUsuarioAsignado, int idPrioridad, int idEstado)
        {
            var ticket = _context.tickets
                .Include(t => t.usuarioE)
                .Include(t => t.prioridad)
                .FirstOrDefault(t => t.id_ticket == id);

            if (ticket != null)
            {
                ticket.id_usuarioE = idUsuarioAsignado;
                ticket.id_prioridad = idPrioridad;
                ticket.id_estado = idEstado;
                _context.SaveChanges();
            }

            var totalTickets = _context.tickets.Count();
            var ticketsAbiertos = _context.tickets.Where(t => t.id_estado == 1).Count();
            var ticketsEnProgreso = _context.tickets.Where(t => t.id_estado == 2).Count();
            var ticketsResueltos = _context.tickets.Where(t => t.id_estado == 4).Count();

            var clientes = _context.usuarios.Where(u => u.roles == "Cliente").ToList();
            var usuarios = _context.usuarios.ToList();
            var tickets = _context.tickets
                .Where(t => t.id_estado != 5)
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

            _context.SaveChanges();

            return Ok(idEstado);
        }


        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var usuario = _context.usuarios.FirstOrDefault(u => u.id_usuarios == id);

            if (usuario == null)
                return NotFound();

          
            var comentarios = _context.comentarios.Where(c => c.id_usuarios == id).ToList();
            _context.comentarios.RemoveRange(comentarios);

            var notificaciones = _context.notificaciones.Where(n => n.id_usuarios == id).ToList();
            _context.notificaciones.RemoveRange(notificaciones);

            var historial = _context.historial.Where(h => h.id_ticket != null &&
                (_context.tickets.Any(t => t.id_ticket == h.id_ticket &&
                (t.id_usuarioC == id || t.id_usuarioE == id)))).ToList();
            _context.historial.RemoveRange(historial);

      
            var ticketsCliente = _context.tickets.Where(t => t.id_usuarioC == id).ToList();
            _context.tickets.RemoveRange(ticketsCliente);

            var ticketsTecnico = _context.tickets.Where(t => t.id_usuarioE == id).ToList();
            _context.tickets.RemoveRange(ticketsTecnico);

          
            _context.usuarios.Remove(usuario);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }









    }
}
