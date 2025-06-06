using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketsF.Models;
using System.Linq;

namespace TicketsF.Controllers
{
    public class TecnicoController1 : Controller
    {
        private readonly ticketsDbContext _context;

        public TecnicoController1(ticketsDbContext context)
        {
            _context = context;
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
    var ticket = _context.tickets.FirstOrDefault(t => t.id_ticket == id);

    if (ticket == null)
        return NotFound();

    ticket.id_estado = idEstado;
    ticket.id_prioridad = idPrioridad;

    _context.SaveChanges();

    return Ok();
}

    }
}
