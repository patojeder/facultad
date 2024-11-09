public class ModificarPresupuestoViewModel
{
    public Presupuesto Presupuesto { get; set; }  // Presupuesto actual
    public List<Cliente> Clientes { get; set; }    // Lista de clientes para el desplegable
    public int ClienteIdSeleccionado { get; set; } // ID del cliente actualmente seleccionado
}
