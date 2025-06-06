using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace TicketsF.Servicios
{
    public class AutenticadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var idUsuario = session.GetInt32("id_usuarios");

            // Si no hay sesión y NO estamos en el login, redirige
            string controller = context.RouteData.Values["controller"]?.ToString();
            string action = context.RouteData.Values["action"]?.ToString();

            if (!controller.Equals("Home", System.StringComparison.OrdinalIgnoreCase) ||
                !action.Equals("Index", System.StringComparison.OrdinalIgnoreCase))
            {
                if (!idUsuario.HasValue)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
            }

            base.OnActionExecuting(context);
        }
    }
}
