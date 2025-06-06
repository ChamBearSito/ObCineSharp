﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Genero { get; set; }
        public float? Clasificacion { get; set; }
        public string? Duracion { get; set; }
        public string? Sinopsis { get; set; }

        [NotMapped]
        public List<Horario>? OpcionesModeloHorarios { get; set; }

    }
}
