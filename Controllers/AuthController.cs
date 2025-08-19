using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using AquaSort.Api.Data;
using AquaSort.Api.Dtos;
using AquaSort.Api.Models;


namespace AquaSort.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AquaSortContext _context;

        public AuthController(AquaSortContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            var passwordHash = Sha256(login.Password);

            var user = _context.Usuarios
                .FirstOrDefault(u => u.usuario == login.Username && u.contraseña == passwordHash);

            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            // Guardar datos en sesión
            HttpContext.Session.SetString("UserId", user.id.ToString());
            HttpContext.Session.SetString("Username", user.usuario);

        return Ok(new { message = "Inicio de sesión exitoso", user = new { user.id, user.usuario, Rol = user.Rol } });
        }

        private string Sha256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        [HttpGet("profile")]
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = _context.Usuarios.Find(int.Parse(userId));
            if (user == null) return NotFound();

            return Ok(new { user.id, user.usuario });
        }

        [HttpPost("register")]
public IActionResult Register([FromBody] RegisterDto newUser)
{
    // Verificar si el usuario ya existe
    if (_context.Usuarios.Any(u => u.usuario == newUser.Username || u.email == newUser.Email))
    {
        return BadRequest(new { message = "El usuario o email ya está registrado" });
    }

    var usuario = new Usuario
    {
        usuario = newUser.Username,
        email = newUser.Email,
        contraseña = Sha256(newUser.Password),
        Rol = 2
    };

    _context.Usuarios.Add(usuario);
    _context.SaveChanges();

    return Ok(new { message = "Registro exitoso", user = usuario });
}


    }
}
