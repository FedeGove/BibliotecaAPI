using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BibliotecaAPI.Controllers;
[ApiController]
[Route("api/[controller]")]

public class AuthController : ControllerBase
{
    private readonly BibliotecaContext _context;
    private readonly IConfiguration _configuration;
    
    public AuthController(BibliotecaContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register([FromBody] LoginRequest request)
    {
        var esistente = await _context.Utenti.
            FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (esistente != null) return BadRequest("Username già esistente");
        
        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var utente = new Utente
        {
            Username = request.Username,
            PasswordHash = hash,
            Ruolo = "Utente"
        };

        _context.Utenti.Add(utente);
        await _context.SaveChangesAsync();
        return Ok(utente);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var utente = await _context.Utenti.
            FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (utente == null) return Unauthorized("L'utente non esiste");
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, utente.PasswordHash)) return Unauthorized("Password errata");
        
        var jwtKey = _configuration["Jwt:Key"];

        if (string.IsNullOrWhiteSpace(jwtKey))
        {
            return StatusCode(500, "JWT key non configurata.");
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey)
        );
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, utente.Username),
            new Claim(ClaimTypes.Role, utente.Ruolo),
            new Claim(ClaimTypes.NameIdentifier, utente.Id.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: creds
        );

        return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
    }
}