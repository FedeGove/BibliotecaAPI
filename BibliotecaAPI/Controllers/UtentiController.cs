using BibliotecaAPI.Data;
using BibliotecaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UtentiController : ControllerBase
{
    private readonly BibliotecaContext _context;
    
    public UtentiController(BibliotecaContext context)
    {
        _context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<Utente>>> GetUtenti()
    {
        return await _context.Utenti.ToListAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult<Utente>> GetUtente(int id)
    {
        var utente = await _context.Utenti.FindAsync(id);
        if (utente == null) return NotFound();
        return utente;
    }
    
    [HttpPost]
    public async Task<ActionResult<Utente>> CreateUtente(Utente utente)
    {
        _context.Utenti.Add(utente);
        await _context.SaveChangesAsync();
        return utente;
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUtente(int id)
    {
        var utente = await _context.Utenti.FindAsync(id);
        if (utente == null) return NotFound();
        _context.Utenti.Remove(utente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}