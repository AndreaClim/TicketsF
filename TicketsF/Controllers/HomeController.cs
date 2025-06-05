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

                // Redirigir al Dashboard del Administrador si el usuario es administrador
                if (usuario.roles != null && usuario.roles.id_roles == 1)
                {
                    return RedirectToAction("Index", "Dashboard"); // Redirigir al Dashboard
                }
                // Si no es administrador, redirigir a la vista principal o una p�gina est�ndar
                return RedirectToAction("Index", "Home");
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
