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
        var esistente = await _context.UtentiAuth.
            FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (esistente != null) return BadRequest("Username già esistente");
        
        var hash = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var utente = new UtenteAuth
        {
            Username = request.Username,
            PasswordHash = hash,
            Ruolo = request.Ruolo ?? "utente"
        };

        _context.UtentiAuth.Add(utente);
        await _context.SaveChangesAsync();
        return Ok(utente);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login([FromBody] LoginRequest request)
    {
        var utente = await _context.UtentiAuth.
            FirstOrDefaultAsync(u => u.Username == request.Username);
        
        if (utente == null) return Unauthorized("L'utente non esiste");
        
        if (!BCrypt.Net.BCrypt.Verify(request.Password, utente.PasswordHash)) return Unauthorized("Password errata");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
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