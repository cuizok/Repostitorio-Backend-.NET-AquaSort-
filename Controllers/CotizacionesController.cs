using Microsoft.AspNetCore.Mvc;
using AquaSort.Api.Data;
using AquaSort.Api.Models;
using AquaSort.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace AquaSort.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CotizacionesController : ControllerBase
    {
        private readonly AquaSortContext _context;

        public CotizacionesController(AquaSortContext context)
        {
            _context = context;
        }

        // GET: api/Cotizaciones
        [HttpGet]
        public async Task<IActionResult> GetCotizaciones()
        {
            var cotizaciones = await _context.Cotizaciones.ToListAsync();
            return Ok(cotizaciones);
        }

        // GET: api/Cotizaciones/pendientes
[HttpGet("pendientes")]
public async Task<IActionResult> GetPendientes()
{
    var pendientes = await _context.Cotizaciones
                                   .Where(c => c.Estado == "Pendiente")
                                   .Select(c => new CotizacionDto
                                   {
                                       CantidadProductos = c.CantidadProductos,
                                       Domicilio = c.Domicilio,
                                       TipoInstalacion = c.TipoInstalacion,
                                       Email = c.Email,  // <- importante
                                       FechaSolicitud = c.FechaSolicitud,
                                       Estado = c.Estado
                                   })
                                   .ToListAsync();
    return Ok(pendientes);
}


        // POST: api/Cotizaciones/crear
      // POST: api/Cotizaciones/crear
[HttpPost("crear")]
public async Task<IActionResult> CrearCotizacion([FromBody] CotizacionDto dto)
{


    var cotizacion = new Cotizacion
    {
        CantidadProductos = dto.CantidadProductos,
        Domicilio = dto.Domicilio,
        TipoInstalacion = dto.TipoInstalacion,
        Email = dto.Email, // Se asigna el nuevo campo
        FechaSolicitud = DateTime.Now,
        Estado = "Pendiente"
    };

    _context.Cotizaciones.Add(cotizacion);
    await _context.SaveChangesAsync();

    return Ok(new { message = "Cotización creada", cotizacion });
}
    }
}
