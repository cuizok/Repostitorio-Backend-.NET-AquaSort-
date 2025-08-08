namespace AquaSort.Api.Models
{
    public class SolicitudDto
    {
        public string ClienteNombre { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string Referencia { get; set; }
        public int Cantidad { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
    }
}
