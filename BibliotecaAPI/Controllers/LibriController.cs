using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers;
[Authorize]
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
    public async Task<ActionResult<Libro>> CreateLibro(Libro libro)
    {
        _context.Libri.Add(libro);
        await _context.SaveChangesAsync();
        return libro;
    }

    [HttpPost("{libroId}/prestito/{utenteId}")]
    public async Task<ActionResult<Libro>> ImpostaPrestitoLibro(int libroId, int utenteId)
    {
        var libro = await _context.Libri.FindAsync(libroId);
        var utente = await _context.Utenti.FindAsync(utenteId);
        
        if (libro == null || utente == null) return NotFound();
        if (libro.Disponibile == false) return BadRequest("Libro non disponibile");
        
        libro.Disponibile = false;
        libro.UtenteId = utenteId;
        libro.DataRestituzione = DateTime.UtcNow.AddDays(30);
        
        _context.Libri.Update(libro);
        await _context.SaveChangesAsync();
        return libro;
    }

    [HttpPost("{libroId}/restituzione")]
    public async Task<ActionResult<Libro>> ImpostaRestituzioneLibro(int libroId)
    {
        var libro = await _context.Libri.FindAsync(libroId);
        if (libro == null) return NotFound();
        
        var utente = await _context.Utenti.FindAsync(libro.UtenteId);
        if (utente == null) return NotFound();

        libro.Disponibile = true;
        libro.UtenteId = null;
        libro.DataRestituzione = null;
        
        _context.Libri.Update(libro);
        await _context.SaveChangesAsync();
        return libro;
    }

    
    
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteLibro(int id)
    {
        var libro = await _context.Libri.FindAsync(id);
        if (libro == null) return NotFound();
        _context.Libri.Remove(libro);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
}