namespace PruebaTecnica.Components.Models
{
    public class Prestamo
    {
        public int Id { get; set; }
        public int TipoPrestamo { get; set; }
        public decimal Monto { get; set; }
        public decimal Tasa { get; set; }
    }
}
