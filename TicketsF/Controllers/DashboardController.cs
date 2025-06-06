using Microsoft.AspNetCore.Mvc;
using TicketsF.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TicketsF.Services;

namespace TicketsF.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ticketsDbContext _context;
        private readonly correo _correo;

        public DashboardController(ticketsDbContext context, correo correo)
        {
            _context = context;
            _correo = correo;
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
                .Include(t => t.usuarioC)
                .Include(t => t.usuarioE)
                .Include(t => t.estado)
                .Include(t => t.prioridad)
                .FirstOrDefault(t => t.id_ticket == id);

            if (ticket != null)
            {
                ticket.id_usuarioE = idUsuarioAsignado;
                ticket.id_prioridad = idPrioridad;
                ticket.id_estado = idEstado;
                _context.SaveChanges();

                // 🔁 Recargar todas las relaciones modificadas
                _context.Entry(ticket).Reference(t => t.estado).Load();
                _context.Entry(ticket).Reference(t => t.prioridad).Load();
                _context.Entry(ticket).Reference(t => t.usuarioE).Load(); // <- IMPORTANTE

                // Notificar técnico asignado
                NotificarTecnico(idUsuarioAsignado, ticket.id_ticket);

                // Notificar al usuario creador del ticket sobre cambio de estado
                NotificarCambioEstadoUsuario(ticket);
            }

            return Ok(idEstado);
        }




        // Notificar técnico asignado
        private void NotificarTecnico(int idTecnico, int idTicket)
        {
            var tecnico = _context.usuarios.FirstOrDefault(u => u.id_usuarios == idTecnico && u.roles == "Técnico");
            var ticketExiste = _context.tickets.Any(t => t.id_ticket == idTicket);

            if (tecnico != null && ticketExiste)
            {
                var mensaje = $"Se te ha asignado el ticket #{idTicket}. Revisa tu panel técnico.";

                _context.notificaciones.Add(new notificaciones
                {
                    id_usuarios = tecnico.id_usuarios,
                    id_ticket = idTicket,
                    mensaje = mensaje,
                    fecha_envio = DateTime.Now
                });

                _context.SaveChanges();
            }
        }

        // Notificar al usuario creador del ticket sobre cambio de estado
        private void NotificarCambioEstadoUsuario(tickets ticket)
        {
            if (ticket.usuarioC != null && !string.IsNullOrEmpty(ticket.usuarioC.correo))
            {
                string nombreTecnico = ticket.usuarioE != null ? $"{ticket.usuarioE.nombre} {ticket.usuarioE.apellido}" : "No asignado";
                string correoTecnico = ticket.usuarioE?.correo ?? "N/A";
                string nombreEstado = ticket.estado?.nombre ?? "Desconocido";
                string nombrePrioridad = ticket.prioridad?.nombre ?? "Desconocida";

                string asuntoCorreo = $"Actualización del estado del ticket #{ticket.id_ticket}";

                string cuerpoCorreo = $@"
────────────────────────────────────
         ACTUALIZACIÓN DE TICKET
────────────────────────────────────

Hola {ticket.usuarioC.nombre},

Te informamos que tu ticket ha sido actualizado.

🆔 Ticket: #{ticket.id_ticket}
📄 Título: {ticket.titulo}

📌 Estado actual: {nombreEstado}
⚠️ Prioridad: {nombrePrioridad}

👨‍💼 Usuario asignado: {nombreTecnico}
📧 Correo de contacto: {correoTecnico}

Si tienes dudas o necesitas más información, puedes responder a este correo.

────────────────────────────────────
Soporte Técnico – TicketsF
";

                _correo.enviar(ticket.usuarioC.correo, asuntoCorreo, cuerpoCorreo);
            }
        }



        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var usuario = _context.usuarios.FirstOrDefault(u => u.id_usuarios == id);

            if (usuario == null)
                return NotFound();

            var comentarios = _context.comentarios.Where(c => c.id_usuarios == id).ToList();
            _context.comentarios.RemoveRange(comentarios);

            var ticketsCliente = _context.tickets.Where(t => t.id_usuarioC == id).ToList();
            foreach (var ticket in ticketsCliente)
            {
                var notifs = _context.notificaciones.Where(n => n.id_ticket == ticket.id_ticket).ToList();
                _context.notificaciones.RemoveRange(notifs);
            }
            _context.tickets.RemoveRange(ticketsCliente);

            var ticketsTecnico = _context.tickets.Where(t => t.id_usuarioE == id).ToList();
            foreach (var ticket in ticketsTecnico)
            {
                var notifs = _context.notificaciones.Where(n => n.id_ticket == ticket.id_ticket).ToList();
                _context.notificaciones.RemoveRange(notifs);
            }
            _context.tickets.RemoveRange(ticketsTecnico);

            var historial = _context.historial.Where(h => h.id_ticket != null &&
                (_context.tickets.Any(t => t.id_ticket == h.id_ticket &&
                (t.id_usuarioC == id || t.id_usuarioE == id)))).ToList();
            _context.historial.RemoveRange(historial);

            _context.usuarios.Remove(usuario);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Editar(usuarios usuarioActualizado)
        {
            if (ModelState.IsValid)
            {
                var usuario = _context.usuarios.FirstOrDefault(u => u.id_usuarios == usuarioActualizado.id_usuarios);
                if (usuario != null)
                {
                    usuario.nombre = usuarioActualizado.nombre;
                    usuario.apellido = usuarioActualizado.apellido;
                    usuario.correo = usuarioActualizado.correo;
                    usuario.telefono = usuarioActualizado.telefono;
                    usuario.autenticacion = usuarioActualizado.autenticacion;
                    usuario.roles = usuarioActualizado.roles;

                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CrearUsuario(usuarios nuevoUsuario)
        {
            _context.usuarios.Add(nuevoUsuario);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
