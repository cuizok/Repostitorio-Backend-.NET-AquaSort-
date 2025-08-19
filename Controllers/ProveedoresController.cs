using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AquaSort.Api.Data;
using AquaSort.Api.Models;
using System;
using System.Threading.Tasks;

namespace AquaSort.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProveedoresController : ControllerBase
    {
        private readonly AquaSortContext _context;

        public ProveedoresController(AquaSortContext context)
        {
            _context = context;
        }

        // GET: api/Proveedores
        [HttpGet]
        public async Task<IActionResult> GetProveedores()
        {
            try
            {
                var proveedores = await _context.Proveedores.ToListAsync();
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener proveedores: {ex.Message}");
            }
        }

        // POST: api/Proveedores
        [HttpPost]
        public async Task<IActionResult> CrearProveedor([FromBody] Proveedor proveedor)
        {
            try
            {
                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();
                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear proveedor: {ex.Message}");
            }
        }

        // PUT: api/Proveedores/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProveedor(int id, [FromBody] Proveedor proveedor)
        {
            try
            {
                var proveedorExistente = await _context.Proveedores.FindAsync(id);
                if (proveedorExistente == null)
                    return NotFound($"Proveedor con ID {id} no encontrado.");

                proveedorExistente.Nombre = proveedor.Nombre;
                proveedorExistente.Telefono = proveedor.Telefono;
                proveedorExistente.Email = proveedor.Email;
                proveedorExistente.Direccion = proveedor.Direccion;
                proveedorExistente.Estado = proveedor.Estado;

                _context.Proveedores.Update(proveedorExistente);
                await _context.SaveChangesAsync();

                return Ok(proveedorExistente);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al actualizar proveedor: {ex.Message}");
            }
        }

        // DELETE: api/Proveedores/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProveedor(int id)
        {
            try
            {
                var proveedor = await _context.Proveedores.FindAsync(id);
                if (proveedor == null)
                    return NotFound($"Proveedor con ID {id} no encontrado.");

                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Proveedor eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al eliminar proveedor: {ex.Message}");
            }
        }
    }
}
