public class Cliente
{
    private int id;
    private string nombre;
    private string email;
    private string telefono;

    public Cliente(){}

    public Cliente(int id, string nombre, string email, string telefono)
    {
        this.id = id;
        this.nombre = nombre;
        this.email = email;
        this.telefono = telefono;
    }

    public int Id { get => id; set => id = value; }
    public string Nombre { get => nombre; set => nombre = value; }
    public string Email { get => email; set => email = value; }
    public string Telefono { get => telefono; set => telefono = value; }
}