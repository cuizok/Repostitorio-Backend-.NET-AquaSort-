using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

public class StockUpdateDto
{
    public int id_producto { get; set; }
    public int cantidadAgregar { get; set; }
}

[ApiController]
[Route("Inventario")]
public class InventarioController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public InventarioController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPut("actualizar-stock")]
    public IActionResult ActualizarStock([FromBody] StockUpdateDto stockUpdate)
    {
        if (stockUpdate == null || stockUpdate.id_producto <= 0 || stockUpdate.cantidadAgregar <= 0)
        {
            return BadRequest("Datos de entrada inválidos.");
        }

        try
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string sql = "UPDATE Producto SET Cantidad = Cantidad + @CantidadAgregar WHERE Id_producto = @Id_producto";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id_producto", stockUpdate.id_producto);
                    command.Parameters.AddWithValue("@CantidadAgregar", stockUpdate.cantidadAgregar);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        return Ok($"Stock actualizado correctamente para el producto con ID {stockUpdate.id_producto}.");
                    else
                        return NotFound($"Producto con ID {stockUpdate.id_producto} no encontrado.");
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error al actualizar el stock: {ex.Message}");
        }
    }
}
