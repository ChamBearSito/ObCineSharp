using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using Obligatorio.Datos;
using Obligatorio.Models;
using System.Web;

namespace Obligatorio.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public ActionResult Logueo()
        {
            return View();
        }
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        // Acción para mostrar la vista de inicio de sesión
        public ActionResult Logueo(Usuario u)
        {
            Console.WriteLine(u);
            if (u != null)
            {
                var Usuario = _context.Usuarios.FirstOrDefault(obj => obj.Correo == u.Correo);
                if (Usuario != null)
                {
                    if (u.Password != Usuario.Password)
                    {
                        TempData["mensajeError"] = "La contraseña es incorrecta!";
                    }
                    else
                    {
                        Response.Cookies.Append("UsuarioCookie", Usuario.ToJson());
                        
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    TempData["mensajeError"] = "No existe el usuario especificado";
                }
            }
            else
            {
                TempData["mensajeError"] = "Ingrese la clave y contrasenia je";
            }
            return View();

        }
        [HttpPost]
        // Acción para mostrar la vista de inicio de sesión
        public async Task<IActionResult> Registro(Usuario u)
        {
            Console.WriteLine(u);
            if (u != null)
            {
                var Usuario = _context.Usuarios.FirstOrDefault(obj => obj.Correo == u.Correo);
                if (Usuario != null)
                {
                    TempData["mensajeError"] = "El correo ya existe!";
                }
                else 
                {
                    if (ModelState.IsValid)
                    {
                        _context.Add(u);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Logueo));
                    }
                    return View(u);
                }
            }
            else
            {
                TempData["mensajeError"] = "Complete los datos!";
            }
            return View();

        }
    }
}
