public class ModificarPresupuestoViewModel
{

     // Constructor sin parámetros requerido para model binding
    public ModificarPresupuestoViewModel()
    {
        Presupuesto = new Presupuesto();
        Clientes = new List<Cliente>();
        Productos = new List<Producto>();
    }

    public Presupuesto Presupuesto { get; set; }  // Presupuesto actual
    public List<Cliente> Clientes { get; set; }    // Lista de clientes para el desplegable
    public int ClienteIdSeleccionado { get; set; } // ID del cliente actualmente seleccionado
    public List<Producto> Productos { get; set; }
}
