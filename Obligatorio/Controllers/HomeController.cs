using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Obligatorio.Datos;
using Obligatorio.Models;
using System.Diagnostics;

namespace Obligatorio.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var laCock=Request.Cookies["UsuarioCookie"];
            ViewBag.UsuarioCookie = JsonConvert.DeserializeObject<Usuario>(laCock!.ToString());
            var losHorarios = _context.Horarios
                .Include(h => h.Pelicula)
                .Where(h => DateTime.Now.CompareTo(h.Fecha)<=0)
                .ToArray();

            var si = _context.Peliculas.ToArray();
            var lasPelis =Array.FindAll(_context.Peliculas.ToArray(),
                x=> losHorarios.FirstOrDefault(y=>y.Pelicula!.Id==x.Id)!=null);
            ViewBag.Cartelera = lasPelis;
            return View();
             
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}