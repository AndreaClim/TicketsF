using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TicketsF.Models;
using System.Linq;

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
        [HttpGet]
        public IActionResult Index()
        {
            return View(); // Simplemente muestra la vista de inicio de sesi�n
        }

        [HttpPost]
        public IActionResult Index(string correo, string contrasenia)
        {
            // Verificar si el formulario ha sido enviado y si los datos son v�lidos
            if (string.IsNullOrEmpty(correo) || string.IsNullOrEmpty(contrasenia))
            {
                ViewData["Error"] = "Por favor, ingrese el correo y la contrase�a.";
                return View();
            }

            // Verificar que el usuario existe con el correo y la contrase�a
            var usuario = _context.usuarios
                .FirstOrDefault(u => u.correo == correo && u.contrasenia == contrasenia);

            if (usuario != null)
            {
                // Si el usuario existe, guardar su id en la sesi�n
                HttpContext.Session.SetInt32("id_usuarios", usuario.id_usuario);
                HttpContext.Session.SetString("correo", usuario.correo);

                // Redirigir al Dashboard del Administrador si el usuario tiene rol "Administrador"
                if (usuario.roles == "Administrador")
                {
                    return RedirectToAction("Index", "Dashboard"); // Redirigir al Dashboard
                }

                // Si el usuario tiene rol "T�cnico" o "Cliente", puedes redirigir a su vista respectiva
                if (usuario.roles == "T�cnico" || usuario.roles == "Cliente")
                {
                    return RedirectToAction("Index", "Home"); // O redirigir a una vista est�ndar si lo prefieres
                }
            }
            else
            {
                // Si las credenciales son incorrectas, mostrar un mensaje de error
                ViewData["Error"] = "Correo o contrase�a incorrectos.";
            }

            return View(); // Si la validaci�n falla, vuelve a mostrar el formulario con el error
        }
    }
}
