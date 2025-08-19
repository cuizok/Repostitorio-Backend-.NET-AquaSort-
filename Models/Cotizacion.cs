namespace AquaSort.Api.Models
{
public class Cotizacion
{
    public int Id { get; set; }
    public int CantidadProductos { get; set; }
    public string Domicilio { get; set; }
    public string TipoInstalacion { get; set; } 
    public DateTime FechaSolicitud { get; set; }
    public string Estado { get; set; } = "Pendiente";
    public string Email { get; set; } // Nuevo campo
}

}
