using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using TicketsF.Models;

namespace TicketsF.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ticketsDbContext _context;

        public UsuariosController(ticketsDbContext context)
        {
            _context = context;
        }

        // GET: Vista para generar un ticket
        public IActionResult GenerarTicket()
        {
            if (HttpContext.Session.GetInt32("id_usuarios") == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        // POST: Guardar ticket en la base de datos
        [HttpPost]
        public IActionResult GenerarTicket(string asunto, string descripcionProblema, string appName, string prioridad)
        {
            int? userId = HttpContext.Session.GetInt32("id_usuarios");

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (string.IsNullOrEmpty(asunto) || string.IsNullOrEmpty(descripcionProblema))
            {
                ViewData["Error"] = "Asunto y descripción son obligatorios.";
                return View();
            }

            var ticket = new tickets
            {
                titulo = asunto,
                descripcion = descripcionProblema,
                nombre_app = appName,
                fecha_creacion = DateTime.Now,
                id_usuarioC = userId.Value,
                id_usuarioE = null, // Asignación futura
                id_cat = 1, // Por defecto si no hay selección
                id_estado = 1, // Asumimos 1 = "Pendiente"
                id_prioridad = prioridad switch
                {
                    "critico" => 1,
                    "importante" => 2,
                    "baja" => 3,
                    _ => 3
                }
            };

            _context.tickets.Add(ticket);
            _context.SaveChanges();

            TempData["Success"] = "¡Ticket generado correctamente!";
            return RedirectToAction("GenerarTicket");
        }
    }
}