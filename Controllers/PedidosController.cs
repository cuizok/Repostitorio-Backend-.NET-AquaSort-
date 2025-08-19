using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AquaSort.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PedidosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("cliente/{clienteNombre}")]
        public IActionResult GetPedidos(string clienteNombre)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                var pedidos = new List<object>();

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT Id, CodigoPedido, ClienteNombre, Telefono, Direccion, Referencia,
                               Fecha, Cantidad, Total, MetodoPago, Estado
                        FROM Solicitudes
                        WHERE ClienteNombre = @ClienteNombre
                        ORDER BY Fecha DESC";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ClienteNombre", clienteNombre);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pedidos.Add(new
                                {
                                    idPedidos = reader.GetInt32(0),
                                    codigo = reader.GetString(1),
                                    cliente = reader.GetString(2),
                                    telefono = reader.GetString(3),
                                    direccion = reader.GetString(4),
                                    referencia = reader.GetString(5),
                                    fecha_pedido = reader.GetDateTime(6),
                                    cantidad = reader.GetInt32(7),
                                    total = reader.GetDecimal(8),
                                    metodoPago = reader.GetString(9),
                                    estatus = reader.GetString(10),
                                    detalles = new List<object>() // Si quieres más detalle de productos
                                });
                            }
                        }
                    }
                }

                return Ok(pedidos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener pedidos: {ex.Message}");
            }
        }
    }
}
