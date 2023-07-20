using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Obligatorio.Datos;
using Obligatorio.Models;

namespace Obligatorio.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            return _context.Reservas != null ?
                         View(await _context.Reservas
                         .Include(h => h.Horario)
                         .Include(h => h.Horario!.Pelicula)
                         .Include(h => h.Horario!.Sala)
                         .Include(h => h.Usuario).ToListAsync()) :
                         Problem("Entity set 'ApplicationDbContext.Reservas'  is null.");
           
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create(Horario hor)
        {
            var laH=_context.Horarios
                .Include(h=>h.Pelicula)
                .Include(h=>h.Sala)
                .FirstOrDefault(h => h.Id == hor.Id);
            
            var lasR = _context.Reservas
                         .Include(h => h.Horario)
                         .Where(h => h.Horario!.Id==laH!.Id)
                         .ToList();

            Response.Cookies.Append("HorarioCookie", laH!.Id.ToString());
            ViewBag.Horario = laH;

            var AsientosOcu=new List<string>();
            foreach (Reserva laR in lasR)
            {
                foreach (string asi in laR.Asientos!.Split(','))
                {
                    AsientosOcu.Add(asi);
                }
            }

            ViewBag.AsientosOcupados = AsientosOcu;
            return View();
        }

        public bool Validacion(string Asientos, Horario h)
        {
            var listaReservas = _context.Reservas.ToList();

            bool comprobarAsientos(Reserva r)
            {
                var lista = r.Asientos!.Split(',');
                foreach(string item in lista)
                {
                    foreach(string subItem in Asientos.Split(','))
                    {
                        if (subItem == item)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }

            listaReservas = listaReservas.FindAll(c =>
                c.Horario!.Id == h.Id &&
                comprobarAsientos(c)
            );

            if (listaReservas.Count > 0)
            {
                return false;
            }
            return true;
        }

        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Asientos)
        {
            Reserva reserva = new Reserva();
            //HORARIO
            var laCookH = Request.Cookies["HorarioCookie"];
            //Horario elHorario= JsonConvert.DeserializeObject<Horario>(laCookH.ToString());
            reserva.Horario = Array.Find(_context.Horarios.ToArray(), x => x.Id == int.Parse(laCookH!));

            if (!Validacion(Asientos, reserva.Horario!))
            {
                TempData["mensajeErrorReserva"] = "Seleccione asientos disponibles";
                return RedirectToAction("Create","Reservas");
            }

            //Usuario
            var laCookU = Request.Cookies["UsuarioCookie"];
            Usuario elUsuario = JsonConvert.DeserializeObject<Usuario>(laCookU!.ToString())!;
            reserva.Usuario = Array.Find(_context.Usuarios.ToArray(), x => x.Id == elUsuario.Id);

            reserva.Asientos = Asientos;
            if (ModelState.IsValid)
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(reserva);
        }

        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
            {
                return NotFound();
            }
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Asientos")] Reserva reserva)
        {
            if (id != reserva.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reserva);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservaExists(reserva.Id))
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
            return View(reserva);
        }

        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Reservas == null)
            {
                return NotFound();
            }

            var reserva = await _context.Reservas
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reserva == null)
            {
                return NotFound();
            }

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Reservas == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Reservas'  is null.");
            }
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
          return (_context.Reservas?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
