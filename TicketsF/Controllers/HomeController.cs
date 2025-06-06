using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TicketsF.Models;
using System.Linq;
using TicketsF.Servicios;

namespace TicketsF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ticketsDbContext _context;

        public HomeController(ticketsDbContext context)
        {
            _context = context;
        }

        // Acci�n para manejar tanto el GET como el POST del inicio de sesi�n
        [Autenticado]
        [HttpGet]
        public IActionResult Index()
        {
            return View(); 
        }

        [Autenticado]
        [HttpPost]
        public IActionResult Index(string correo, string contrasenia)
        {
            
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
            {
                ViewData["Error"] = "Por favor, ingrese el correo y la contrase�a.";
                return View();
            }

            
            var usuario = _context.usuarios
                .FirstOrDefault(u => u.correo == correo && u.contrasenia == contrasenia);

            if (usuario != null)
            {
                
                HttpContext.Session.SetInt32("id_usuarios", usuario.id_usuarios);
                HttpContext.Session.SetString("correo", usuario.correo);

               
                if (usuario.roles == "Administrador")
                {
                    return RedirectToAction("Index", "Dashboard");
                }

                
                if (usuario.roles == "T�cnico" || usuario.roles == "Cliente")
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                
                ViewData["Error"] = "Correo o contrase�a incorrectos.";
            }

            return View();
        }
    }
}
