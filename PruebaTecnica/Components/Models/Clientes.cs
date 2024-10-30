using System.ComponentModel.DataAnnotations;

namespace PruebaTecnica.Components.Models
{
    public class Clientes
    {
        public int? Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Identificacion { get; set; }
        public char? Genero { get; set; }
        public char? Es_empleado { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public char? Estado { get; set; }
        public int? PerfilEmpleado { get; set; }
        public int? Oficial { get; set; }
        public DateTime Fecha_creacion { get; set; }
        public DateTime Fecha_modificacion { get; set; }
    }


}
