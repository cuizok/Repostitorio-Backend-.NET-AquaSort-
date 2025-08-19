using System.ComponentModel.DataAnnotations.Schema;

namespace AquaSort.Api.Models
{
    [Table("Proveedor")] // <- Esto indica a EF que use exactamente esta tabla
    public class Proveedor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Estado { get; set; } = "Activo";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
