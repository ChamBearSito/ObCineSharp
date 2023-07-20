using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Obligatorio.Models
{
    [PrimaryKey( nameof(Id))]
    public class Sala
    {
        public int Id { get; set; }

        [Key]
        public int? Numero { get; set; }
        public int? Capacidad { get; set; }
    }
}
