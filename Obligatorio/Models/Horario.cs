using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio.Models
{
    public class Horario
    {
        public int Id { get; set; }
        public Pelicula? Pelicula { get; set; }

        public DateTime Fecha { get; set; }

        public Sala? Sala { get; set; }
        
        [NotMapped]
        public List<SelectListItem>? OpcionesModeloPelicula{ get; set; }
        [NotMapped]
        public List<SelectListItem>? OpcionesModeloSala { get; set; }
    }
}
