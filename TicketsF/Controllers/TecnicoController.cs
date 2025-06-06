using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsF.Models;
using System.Linq;
using TicketsF.Services;

namespace TicketsF.Controllers
{
    public class TecnicoController1 : Controller
    {
        private readonly ticketsDbContext _context;
        private readonly correo _correo;

        public TecnicoController1(ticketsDbContext context, correo correo)
        {
            _context = context;
            _correo = correo;
        }

        public IActionResult Index()
        {
            var idTecnico = HttpContext.Session.GetInt32("id_usuarios");
            if (!idTecnico.HasValue)
                return RedirectToAction("Index", "Home");

            // Tickets asignados a este técnico
            var tickets = _context.tickets
            .Where(t => t.id_usuarioE == idTecnico && t.id_estado != 4 && t.id_estado != 5)
            .Include(t => t.estado)
            .Include(t => t.prioridad)
            .Include(t => t.categoria)
            .ToList();


            // Conteos filtrados por técnico
            ViewBag.TicketsResueltos = tickets.Count(t => t.id_estado == 4);
            ViewBag.TicketsAbiertos = tickets.Count(t => t.id_estado == 1);
            ViewBag.TicketsEnProgreso = tickets.Count(t => t.id_estado == 2);
            ViewBag.TicketsEnEspera = tickets.Count(t => t.id_estado == 3);
            ViewBag.TicketsCerrados = _context.tickets.Count(t => t.id_usuarioE == idTecnico && t.id_estado == 5);

            ViewBag.Tickets = tickets;
            ViewBag.Estados = _context.estado.ToList();
            ViewBag.Prioridades = _context.prioridad.ToList();
            ViewBag.Notificaciones = _context.notificaciones
                .Where(n => n.id_usuarios == idTecnico)
                .OrderByDescending(n => n.fecha_envio)
                .ToList();

            return View();
        }

        [HttpPost]

        public IActionResult ActualizarEstado(int id, int idEstado, int idPrioridad)
        {
            var ticket = _context.tickets
                .Include(t => t.usuarioC)   // Cliente
                .Include(t => t.usuarioE)   // Técnico asignado
                .Include(t => t.estado)
                .Include(t => t.prioridad)
                .FirstOrDefault(t => t.id_ticket == id);

            if (ticket == null)
                return NotFound();

            ticket.id_estado = idEstado;
            ticket.id_prioridad = idPrioridad;

            _context.SaveChanges();

            // 🔁 Asegurarse que las relaciones se recarguen
            _context.Entry(ticket).Reference(t => t.estado).Load();
            _context.Entry(ticket).Reference(t => t.prioridad).Load();
            _context.Entry(ticket).Reference(t => t.usuarioE).Load();

            // Enviar notificación por correo al cliente
            NotificarCambioEstadoUsuario(ticket);

            return Ok();
        }


        private void NotificarCambioEstadoUsuario(tickets ticket)
        {
            if (ticket.usuarioC != null && !string.IsNullOrEmpty(ticket.usuarioC.correo))
            {
                string nombreTecnico = ticket.usuarioE != null
                    ? $"{ticket.usuarioE.nombre} {ticket.usuarioE.apellido}"
                    : "No asignado";

                string correoTecnico = ticket.usuarioE?.correo ?? "No disponible";
                string nombreEstado = ticket.estado?.nombre ?? "Desconocido";
                string nombrePrioridad = ticket.prioridad?.nombre ?? "Desconocida";

                string asunto = $"🔔 Actualización de su ticket #{ticket.id_ticket}";

                string cuerpo = $@"
===============================================
       📢 ACTUALIZACIÓN DE SU TICKET
===============================================

Hola {ticket.usuarioC.nombre} 👋,

Le informamos que su ticket con título:
'{ticket.titulo}'

➡️ Estado actual    : {nombreEstado}
⚠️ Prioridad        : {nombrePrioridad}

👨‍🔧 Técnico asignado: {nombreTecnico}
📧 Correo contacto  : {correoTecnico}

Gracias por confiar en nuestro servicio de soporte.

===============================================
           💼 Equipo de Soporte Técnico
===============================================";

                _correo.enviar(ticket.usuarioC.correo, asunto, cuerpo);
            }
        }




    }
}
