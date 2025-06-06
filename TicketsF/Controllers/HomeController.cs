using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TicketsF.Models;
using System.Linq;
using TicketsF.Servicios;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.Numerics;

namespace TicketsF.Controllers
{
    public class HomeController : Controller
    {
        private readonly ticketsDbContext _context;

        public HomeController(ticketsDbContext context)
        {
            _context = context;
        }

        // Acción para manejar tanto el GET como el POST del inicio de sesión
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
                ViewData["Error"] = "Por favor, ingrese el correo y la contraseña.";
                return View();
            }


            var usuario = _context.usuarios
        .FirstOrDefault(u => u.correo == correo && u.contrasenia == contrasenia);

            if (usuario != null)
            {
                HttpContext.Session.SetInt32("id_usuarios", usuario.id_usuarios);
                HttpContext.Session.SetString("correo", usuario.correo);

                // Redirección basada en rol
                switch (usuario.roles)
                {
                    case "Administrador":
                        return RedirectToAction("Index", "Dashboard");

                    case "Técnico":
                        return RedirectToAction("Index", "TecnicoController1");


                    case "Cliente":
                        return RedirectToAction("GenerarTicket", "Usuarios");

                    default:
                        ViewData["Error"] = "Rol no válido.";
                        break;
                }
            }
            else
            {
                ViewData["Error"] = "Correo o contraseña incorrectos.";
            }

            return View();
        }








        }
}

