using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace AquaSort.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public InventarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("productos")]
        public IActionResult ObtenerProductos()
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                var productos = new List<object>();

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT Id_producto, Descripcion, Cantidad FROM Producto";

                    using (var command = new SqlCommand(sql, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new
                            {
                                Id_producto = reader.GetInt32(0),
                                Descripcion = reader.GetString(1),
                                Cantidad = reader.GetInt32(2)
                            });
                        }
                    }
                }

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener productos: {ex.Message}");
            }
        }

        // También puede estar aquí el método ActualizarStock
    }
}
