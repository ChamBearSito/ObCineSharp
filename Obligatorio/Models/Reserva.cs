using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Obligatorio.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        public Usuario? Usuario { get; set; }
        public Horario? Horario { get; set; }
        public string? Asientos { get; set; }

    }
}
