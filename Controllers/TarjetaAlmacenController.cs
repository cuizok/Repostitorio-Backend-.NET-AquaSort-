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
public IActionResult GetMovimientos(int productoId)
{
    try
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");
        var movimientos = new List<object>();

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = @"SELECT IdMovimiento, Fecha, Entrada, Salida, Costo 
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
        return StatusCode(500, $"Error: {ex.Message}");
    }
}
[HttpPut("{id}")]
public IActionResult ActualizarMovimiento(int id, [FromBody] MovimientoDto mov)
{
    try
    {
        string connectionString = _configuration.GetConnectionString("DefaultConnection");

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sql = @"
                UPDATE TarjetaAlmacen
                SET Entrada = @Entrada,
                    Salida = @Salida,
                    Costo = @Costo
                WHERE IdMovimiento = @Id";

            using (var command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Entrada", mov.Entrada);
                command.Parameters.AddWithValue("@Salida", mov.Salida);
                command.Parameters.AddWithValue("@Costo", mov.Costo);
                command.Parameters.AddWithValue("@Id", id);

                int rows = command.ExecuteNonQuery();
                if (rows == 0) return NotFound("Movimiento no encontrado");
            }
        }

        return Ok();
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Error al actualizar movimiento: {ex.Message}");
    }
}

public class MovimientoDto
{
    public int Entrada { get; set; }
    public int Salida { get; set; }
    public decimal Costo { get; set; }
}


    }

    
}
