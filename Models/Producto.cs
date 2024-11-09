using System.ComponentModel.DataAnnotations;

public class Producto
{
    // Consultar el porque siempre deben ser publicos para que DataAnnotations acceda a las validaciones
    private int idProducto;

    [Required(ErrorMessage = "La descripcion es obligatoria")]
    [StringLength(250, ErrorMessage = "La descripcion no puede tener más de 250 caracteres.")]
    public string Descripcion { get; set; }

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El precio debe ser un valor positivo")]
    public int Precio { get; set; }

    public Producto() {}

    public Producto(int idProducto, string descripcion, int precio)
    {
        this.idProducto = idProducto;
        Descripcion = descripcion;
        Precio = precio;
    }

    public int IdProducto { get => idProducto; set => idProducto = value; }
}
