using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using AquaSort.Api.Models;

namespace AquaSort.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SolicitudesController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SolicitudesController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("crear")]
        public IActionResult CrearSolicitud([FromBody] SolicitudDto dto)
        {
            if (dto == null || dto.Cantidad <= 0 || dto.Total <= 0)
                return BadRequest("Datos inválidos.");

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string codigoPedido;

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var countCommand = new SqlCommand("SELECT COUNT(*) FROM Solicitudes", connection);
                    int totalSolicitudes = (int)countCommand.ExecuteScalar();
                    codigoPedido = "PED" + (totalSolicitudes + 1).ToString("D3");

                    var insertCommand = new SqlCommand(@"
                        INSERT INTO Solicitudes 
                        (CodigoPedido, ClienteNombre, Telefono, Direccion, Referencia, Cantidad, Total, MetodoPago, Estado, Fecha)
                        VALUES 
                        (@CodigoPedido, @ClienteNombre, @Telefono, @Direccion, @Referencia, @Cantidad, @Total, @MetodoPago, @Estado, @Fecha)", connection);

                    insertCommand.Parameters.AddWithValue("@CodigoPedido", codigoPedido);
                    insertCommand.Parameters.AddWithValue("@ClienteNombre", dto.ClienteNombre ?? "Cuitlahuac Ramirez");
                    insertCommand.Parameters.AddWithValue("@Telefono", dto.Telefono);
                    insertCommand.Parameters.AddWithValue("@Direccion", dto.Direccion);
                    insertCommand.Parameters.AddWithValue("@Referencia", dto.Referencia ?? (object)DBNull.Value);
                    insertCommand.Parameters.AddWithValue("@Cantidad", dto.Cantidad);
                    insertCommand.Parameters.AddWithValue("@Total", dto.Total);
                    insertCommand.Parameters.AddWithValue("@MetodoPago", dto.MetodoPago);
                    insertCommand.Parameters.AddWithValue("@Estado", "Pendiente");
                    insertCommand.Parameters.AddWithValue("@Fecha", DateTime.Now);

                    insertCommand.ExecuteNonQuery();
                }

                return Ok(new { success = true, codigoPedido });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la solicitud: {ex.Message}");
            }
        }

        [HttpGet("pendientes")]
        public IActionResult ObtenerSolicitudesPendientes()
        {
            var solicitudes = new List<dynamic>();

            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");

                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    var query = @"SELECT Id, CodigoPedido, ClienteNombre, Telefono, Direccion, Referencia, Fecha, Cantidad, Total 
                                  FROM Solicitudes
                                  WHERE Estado = 'Pendiente'
                                  ORDER BY Fecha DESC";

                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            solicitudes.Add(new
                            {
                                id = reader["Id"],
                                numeroPedido = reader["CodigoPedido"],
                                nombre = reader["ClienteNombre"],
                                telefono = reader["Telefono"],
                                direccion = reader["Direccion"],
                                referencia = reader["Referencia"],
                                fecha = Convert.ToDateTime(reader["Fecha"]).ToString("yyyy-MM-dd HH:mm"),
                                cantidadProductos = reader["Cantidad"],
                                total = reader["Total"]
                            });
                        }
                    }
                }

                return Ok(solicitudes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener solicitudes: {ex.Message}");
            }
        }

        [HttpPut("atender/{id}")]
        public async Task<IActionResult> MarcarComoAtendido(int id)
        {
            var solicitud = await _context.Solicitudes.FindAsync(id);

            if (solicitud == null)
            {
                return NotFound();
            }

            solicitud.Estado = "Atendido";
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Solicitud marcada como atendida" });
        }

    }
}
