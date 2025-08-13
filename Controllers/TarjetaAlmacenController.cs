using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AquaSort.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarjetaAlmacenController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TarjetaAlmacenController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("producto/{productoId}")]
        public IActionResult GetEstadisticaProducto(int productoId)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                var movimientos = new List<object>();

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = @"
                        SELECT IdMovimiento, Fecha, Entrada, Salida, Costo
                        FROM TarjetaAlmacen
                        WHERE ProductoId = @ProductoId
                        ORDER BY Fecha";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ProductoId", productoId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                movimientos.Add(new
                                {
                                    IdMovimiento = reader.GetInt32(0),
                                    Fecha = reader.GetDateTime(1),
                                    Entrada = reader.GetInt32(2),
                                    Salida = reader.GetInt32(3),
                                    Costo = reader.GetDecimal(4)
                                });
                            }
                        }
                    }
                }

                return Ok(movimientos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener movimientos: {ex.Message}");
            }
        }
    }
}
