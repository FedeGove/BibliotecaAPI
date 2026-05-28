namespace BibliotecaAPI.Models;

public class Libro
{
    public int Id { get; set; }
    public string Titolo  { get; set; }
    public string Autore { get; set; }
    public int Anno { get; set; }
    public string Genere { get; set; }
    public bool Disponibile { get; set; }
    
    public DateTime? DataRestituzione { get; set; }
    public int? UtenteId { get; set; }

    public Libro(string titolo, string autore, int anno, string genere, bool disponibile)
    {
        this.Titolo = titolo;
        this.Autore = autore;
        this.Anno = anno;
        this.Disponibile = disponibile;
        this.Genere = genere;
    }

}