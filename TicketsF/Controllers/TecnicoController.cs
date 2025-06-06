using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TicketsF.Models;
using TicketsF.Servicios;
using System.Linq;

namespace TicketsF.Controllers
{
    [Authorize(Roles = "Técnico")]
    public class TecnicoController : Controller
    {
        private readonly ticketsDbContext _context;
        private readonly ICorreoServicio _correo;

        public TecnicoController(ticketsDbContext context, ICorreoServicio correo)
        {
            _context = context;
            _correo = correo;
        }

        public IActionResult Index()
        {
            var idTecnico = HttpContext.Session.GetInt32("id_usuarios");

            var ticketsAsignados = _context.tickets
                .Where(t => t.id_usuarioE == idTecnico && t.id_estado != 5)
                .Include(t => t.usuarioC)
                .Include(t => t.estado)
                .Include(t => t.prioridad)
                .ToList();

            var estados = _context.estado.ToList();
            var prioridades = _context.prioridad.ToList();

            var model = new TecnicoData
            {
                TicketsAsignados = ticketsAsignados,
                Estado = estados,
                Prioridades = prioridades
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult ActualizarEstado(int id, int idEstado, int idPrioridad)
        {
            var ticket = _context.tickets
                .Include(t => t.usuarioC)
                .FirstOrDefault(t => t.id_ticket == id);

            if (ticket != null)
            {
                ticket.id_estado = idEstado;
                ticket.id_prioridad = idPrioridad;
                _context.SaveChanges();

                // Notificación por correo al cliente
                var cliente = ticket.usuarioC;
                if (cliente != null)
                {
                    var mensaje = $"Hola {cliente.nombre}, tu ticket #{ticket.id_ticket} ha cambiado de estado a: {_context.estado.Find(idEstado)?.nombre}";
                    _correo.EnviarCorreo(cliente.correo, "Actualización de Ticket", mensaje);
                }

                return Ok(new { estado = idEstado });
            }

            return NotFound();
        }
    }
}
