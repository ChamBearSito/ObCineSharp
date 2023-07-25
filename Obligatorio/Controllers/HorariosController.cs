using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Obligatorio.Datos;
using Obligatorio.Models;

namespace Obligatorio.Controllers
{
    public class HorariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HorariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Horarios
        public async Task<IActionResult> Index()
        {
              return _context.Horarios != null ? 
                          View(await _context.Horarios
                          .Include(h => h.Pelicula)
                          .Include(h => h.Sala).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Horarios'  is null.");
        }

        // GET: Horarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Horarios == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // GET: Horarios/Create
        public IActionResult Create()
        {
            var modeloSecundarioPelis=_context.Peliculas.ToList();
            var modeloSecundarioSalas = _context.Salas.ToList();
            
            // Crea una lista de elementos SelectListItem para usar en el elemento <select>
            var opcionesP = new List<SelectListItem>();
            foreach (var item in modeloSecundarioPelis)
            {
                opcionesP.Add(new SelectListItem
                {
                    Value = item.Id.ToString(), // Asigna el valor de la propiedad Id del modelo secundario
                    Text = item.Titulo, // Asigna el valor de la propiedad Nombre del modelo secundario
                    Selected = false
                }) ;
            }
            var opcionesS = new List<SelectListItem>();
            foreach (var item in modeloSecundarioSalas)
            {
                opcionesS.Add(new SelectListItem
                {
                    Value = item.Id.ToString(), // Asigna el valor de la propiedad Id del modelo secundario
                    Text = item.Numero.ToString(), // Asigna el valor de la propiedad Nombre del modelo secundario
                    Selected = false
                });
            }
            var modelo = new Horario
            {
                OpcionesModeloPelicula = opcionesP,
                OpcionesModeloSala = opcionesS
            };
            
            return View(modelo);
        }

        // POST: Horarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public async Task<IActionResult> Create(Horario horario, int Pelicula,int Sala)
        {
            var laPeli=Array.Find(_context.Peliculas.ToArray(), x => x.Id == Pelicula);
            horario.Pelicula = laPeli;
            var laSala = Array.Find(_context.Salas.ToArray(), x => x.Id == Sala);
            horario.Sala = laSala;
            foreach (var item in _context.Horarios.Include(h => h.Sala).ToArray())
            {
                if (horario.Fecha.CompareTo(item.Fecha) == 0 && horario.Sala!.Id == item.Sala!.Id)
                {
                    TempData["mensajeErrorHorario"] = "El horario no puede tener la misma fecha en la misma sala";
                    return RedirectToAction("Create", "Horarios");
                }
            }
            Console.WriteLine(horario);
            if (ModelState.IsValid)
            {
                _context.Add(horario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(horario);
        }

        // GET: Horarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Horarios == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios.FindAsync(id);
            if (horario == null)
            {
                return NotFound();
            }
            return View(horario);
        }

        // POST: Horarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Hora")] Horario horario)
        {
            if (id != horario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(horario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HorarioExists(horario.Id))
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
            return View(horario);
        }

        // GET: Horarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Horarios == null)
            {
                return NotFound();
            }

            var horario = await _context.Horarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (horario == null)
            {
                return NotFound();
            }

            return View(horario);
        }

        // POST: Horarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Horarios == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Horarios'  is null.");
            }
            var horario = await _context.Horarios.FindAsync(id);
            if (horario != null)
            {
                _context.Horarios.Remove(horario);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HorarioExists(int id)
        {
          return (_context.Horarios?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
