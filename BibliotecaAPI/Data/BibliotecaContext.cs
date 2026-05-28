using BibliotecaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaAPI.Data;

public class BibliotecaContext : DbContext
{
    public BibliotecaContext(DbContextOptions<BibliotecaContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UtenteAuth>().ToTable("UtenteAuth");
    }
    
    public DbSet<Libro> Libri { get; set; }
    public DbSet<Utente> Utenti { get; set; }
    public DbSet<UtenteAuth> UtentiAuth { get; set; }
}