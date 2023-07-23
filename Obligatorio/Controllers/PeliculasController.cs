using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Obligatorio.Datos;
using Obligatorio.Models;

namespace Obligatorio.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PeliculasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Peliculas
        public async Task<IActionResult> Index(string buscar)
        {
            var laCock = Request.Cookies["UsuarioCookie"];
            ViewBag.UsuarioCookie = JsonConvert.DeserializeObject<Usuario>(laCock!.ToString());

            var peliculaFilter = _context.Peliculas.ToList();

            if (!String.IsNullOrEmpty(buscar))
            {
                if(float.TryParse(buscar, out float buscarnum))
                {
                    peliculaFilter = peliculaFilter.Where(c =>
                    c.Clasificacion!.ToString()!.Contains(buscar) ||
                    c.Clasificacion>=buscarnum
                    ).ToList();
                }
                else
                {
                    peliculaFilter = peliculaFilter.Where(c =>
                    c.Titulo!.ToLower().Contains(buscar.ToLower()) ^
                    c.Genero!.ToLower().Contains(buscar.ToLower())
                    ).ToList();
                }         
            }
            return _context.Peliculas != null ?
                          View(peliculaFilter.ToList()) :
                          Problem("Entity set 'ApplicationDbContext.Usuarios' is null.");
        }
        public ActionResult Details(int Id)
        {
            
            Pelicula peli = _context.Peliculas.Find(Id)!;
            var modeloSecundarioHoras = _context.Horarios
                                        .Include(h => h.Pelicula)
                                        .Include(h => h.Sala)
                                        .Where(h => DateTime.Now.CompareTo(h.Fecha) <= 0)
                                        .ToList();
            var opcionesH = new List<Horario>();

            foreach (var item in modeloSecundarioHoras)
            {
                if (item.Pelicula != null && item.Pelicula.Id == peli.Id)
                {
                    opcionesH.Add(item);
                }
            }

            peli!.OpcionesModeloHorarios = opcionesH;
            return PartialView("DetailsPre", peli);
        }

        // GET: Peliculas/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    var laCock = Request.Cookies["UsuarioCookie"];
        //    ViewBag.UsuarioCookie = JsonConvert.DeserializeObject<Usuario>(laCock!.ToString());

        //    var pelicula = _context.Peliculas
        //        .FirstOrDefault(m => m.Id == id);

        //    var modeloSecundarioHoras =_context.Horarios
        //                                .Include(h => h.Pelicula)
        //                                .Include(h => h.Sala)
        //                                .Where(h => DateTime.Now.CompareTo(h.Fecha) <= 0)
        //                                .ToList();
        //    var opcionesH = new List<Horario>();
        //    foreach (var item in modeloSecundarioHoras)
        //    {
        //        if (item.Pelicula!=null && item.Pelicula.Id==id)
        //        {
        //            opcionesH.Add(item);
        //        }
        //    }

        //    pelicula!.OpcionesModeloHorarios = opcionesH;
        //    return View(pelicula);
        //}

        // GET: Peliculas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Peliculas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula, string Clasificacion)
        {
            float laClas = float.Parse(Clasificacion.Replace('.',','));
            pelicula.Clasificacion = laClas;
            if (ModelState.IsValid)
            {
                _context.Add(pelicula);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Peliculas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Peliculas == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
            {
                return NotFound();
            }
            return View(pelicula);
        }

        // POST: Peliculas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Genero,Clasificacion,Duracion,Sinopsis")] Pelicula pelicula)
        {
            if (id != pelicula.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pelicula);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PeliculaExists(pelicula.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pelicula);
        }

        // GET: Peliculas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Peliculas == null)
            {
                return NotFound();
            }

            var pelicula = await _context.Peliculas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pelicula == null)
            {
                return NotFound();
            }

            return View(pelicula);
        }

        // POST: Peliculas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Peliculas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Peliculas'  is null.");
            }
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula != null)
            {
                _context.Peliculas.Remove(pelicula);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PeliculaExists(int id)
        {
          return (_context.Peliculas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
