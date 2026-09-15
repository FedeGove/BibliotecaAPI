using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers;
[ApiController]
[Route("api/[controller]")]

public class LibriController : ControllerBase
{
    private readonly BibliotecaContext _context;

    public LibriController(BibliotecaContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<Libro>>> GetLibri()
    {
        return await _context.Libri.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Libro>> GetLibro(int id)
    {
        var libro = await _context.Libri.FindAsync(id);
        if (libro == null) return NotFound();
        return libro;
    }
    
    
    
    [HttpPost]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<Libro>> CreateLibro(Libro libro)
    {
        _context.Libri.Add(libro);
        await _context.SaveChangesAsync();
        return libro;
    }

    [HttpPost("{libroId}/prestito")]
    [Authorize]
    public async Task<ActionResult<Libro>> ImpostaPrestitoLibro(int libroId)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized();

        var utenteId = int.Parse(userIdClaim.Value);

        var libro = await _context.Libri.FindAsync(libroId);
        var utente = await _context.Utenti.FindAsync(utenteId);

        if (libro == null || utente == null)
            return NotFound();

        if (!libro.Disponibile)
            return BadRequest("Libro non disponibile");

        libro.Disponibile = false;
        libro.UtenteId = utenteId;
        libro.DataRestituzione = DateTime.UtcNow.AddDays(30);

        await _context.SaveChangesAsync();

        return libro;
    }

    [HttpPost("{libroId}/restituzione")]
    [Authorize]
    public async Task<ActionResult<Libro>> ImpostaRestituzioneLibro(int libroId)
    {
        var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

        if (userIdClaim == null)
            return Unauthorized();

        var utenteId = int.Parse(userIdClaim.Value);

        var libro = await _context.Libri.FindAsync(libroId);

        if (libro == null)
            return NotFound();

        if (libro.Disponibile)
            return BadRequest("Il libro non risulta in prestito");
    
        if (libro.UtenteId != utenteId)
            return Forbid();

        libro.Disponibile = true;
        libro.UtenteId = null;
        libro.DataRestituzione = null;

        await _context.SaveChangesAsync();

        return libro;
    }

    
    
    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult> DeleteLibro(int id)
    {
        var libro = await _context.Libri.FindAsync(id);
        if (libro == null) return NotFound();
        _context.Libri.Remove(libro);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
}