public class PresupuestoViewModel
{
    private int id;
    private List<Producto> productos;
    private List<Cliente> clientes;

    public int Id { get => id; set => id = value; }
    public List<Producto> Productos { get => productos; set => productos = value; }
    public List<Cliente> Clientes { get => clientes; set => clientes = value; }

    // Propiedad para almacenar el ID del cliente seleccionado
    public int ClienteIdSeleccionado { get; set; }

    // Lista para almacenar los IDs de los productos seleccionados
    public List<int> ProductosSeleccionados { get; set; }

    // Diccionario para almacenar la cantidad de cada producto seleccionado
    public Dictionary<int, int> CantidadProductosSeleccionados { get; set; } = new Dictionary<int, int>();

    public PresupuestoViewModel()
    {
        Productos = new List<Producto>();
        Clientes = new List<Cliente>();
        ProductosSeleccionados = new List<int>();
    }
}
