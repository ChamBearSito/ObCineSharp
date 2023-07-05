namespace Obligatorio.Models
{
    public class Horario
    {
        public int Id { get; set; }
        public Pelicula? Pelicula { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public Sala? Sala { get; set; }
    }
}
