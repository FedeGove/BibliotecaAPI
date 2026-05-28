namespace BibliotecaAPI.Models;

public class UtenteAuth
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
}