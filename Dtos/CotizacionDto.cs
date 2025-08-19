namespace AquaSort.Api.Dtos
{
    public class CotizacionDto
    {
        public int CantidadProductos { get; set; }
        public string Domicilio { get; set; }
        public string TipoInstalacion { get; set; }
        public string Email { get; set; }          // <- email del cliente
        public DateTime FechaSolicitud { get; set; } // <- fecha de la solicitud
        public string Estado { get; set; }         // <- estado de la cotización
    }
}



