using System.ComponentModel.DataAnnotations;

public class Cliente
{
    private int id;

    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(150, ErrorMessage = "El nombre debe tener menos de 150 caracteres")] //Le agregue yo
    public string Nombre { get; set; }
    
    [EmailAddress(ErrorMessage = "Debe tener formato de correo electronico")]
    [Required(ErrorMessage = "El email es obligatorio")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "Debe tener formato de número de telefono")]
    [Required(ErrorMessage = "El telefono es obligatorio")]
    public string Telefono { get; set; }

    public Cliente(){}

    public Cliente(int id, string nombre, string email, string telefono)
    {
        this.id = id;
        Nombre = nombre;
        Email = email;
        Telefono = telefono;
    }

    public int Id { get => id; set => id = value; }
}