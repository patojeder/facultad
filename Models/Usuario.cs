public class Usuario
{
    public int Id { get; set; }
    public string Nombre {get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Rol Rol { get; set; }
}

public enum Rol
{
    Admin = 0,
    Cliente = 1,
    Empleado = 2
}