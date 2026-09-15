namespace BibliotecaAPI.Models;

public class Utente
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Ruolo { get; set; } = "utente";

    public Utente() { }
}