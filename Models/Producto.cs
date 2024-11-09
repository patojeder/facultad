using System.ComponentModel.DataAnnotations;

public class Producto
{
    private int idProducto;

    [StringLength(250, ErrorMessage = "La descripcion no puede tener mas de 250 caracteres.")]
    private string descripcion;

    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0, int.MaxValue, ErrorMessage = "El precio debe ser un valor positivo")]
    private int precio;

    public Producto(){}
    public Producto(int idProducto, string descripcion, int precio)
    {
        this.idProducto = idProducto;
        this.descripcion = descripcion;
        this.precio = precio;
    }

    public int IdProducto { get => idProducto; set => idProducto = value; }
    public string Descripcion { get => descripcion; set => descripcion = value; }
    public int Precio { get => precio; set => precio = value; }
}