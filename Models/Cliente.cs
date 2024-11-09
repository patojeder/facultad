using System.ComponentModel.DataAnnotations;

public class Cliente
{
    private int id;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150, ErrorMessage = "El nombre debe tener menos de 150 caracteres")] //Le agregue yo
    private string nombre;
    
    [EmailAddress(ErrorMessage = "Debe tener formato de correo electronico")]
    private string email;

    [Phone(ErrorMessage = "Debe tener formato de número de telefono")]
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