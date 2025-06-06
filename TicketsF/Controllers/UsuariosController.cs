using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using TicketsF.Models;

using TicketsF.Services;
using TicketsF.Servicios;

namespace TicketsF.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ticketsDbContext _context;
        private readonly correo _correo;

        public UsuariosController(ticketsDbContext context, correo correo)
        {
            _context = context;
            _correo = correo;
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
                id_usuarioE = null,
                id_cat = 1,
                id_estado = 1,
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

            // 📧 Enviar correo al usuario logueado
            var usuario = _context.usuarios.FirstOrDefault(u => u.id_usuarios == userId.Value);

            if (usuario != null)
            {
                string asuntoCorreo = "✅ Confirmación de ticket generado";

                string cuerpoCorreo = $@"
============================================
       🎫 CONFIRMACIÓN DE SU TICKET
============================================

Hola {usuario.nombre} 👋,

¡Gracias por contactarnos!

Su ticket con asunto:
'{asunto}'

ha sido generado exitosamente 🆗.

Nuestro equipo pronto lo revisará y dará seguimiento.

Si necesita más información, no dude en contactarnos.

============================================
       💼 Equipo de Soporte Técnico
============================================";

                _correo.enviar(usuario.correo, asuntoCorreo, cuerpoCorreo);
            }


            TempData["Success"] = "¡Ticket generado correctamente!";
            return RedirectToAction("GenerarTicket");
        }
    }
}