using System.Text.Json.Serialization;

public class Presupuesto
{
    private static int autoincremento = 5;
    private int idPresupuesto;
    private string fechaCreacion;
    private Cliente cliente;
    private List<PresupuestoDetalle> detalle;


    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public Cliente Cliente { get => cliente; set => cliente = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

    public Presupuesto(int idPresupuesto, string fechaCreacion)
    {
        this.idPresupuesto = idPresupuesto;
        this.fechaCreacion = fechaCreacion;
        cliente = new Cliente();
        detalle = new List<PresupuestoDetalle>();
    }

    public Presupuesto()
    {
        IdPresupuesto = autoincremento++;
        FechaCreacion = DateTime.Today.ToString();
        detalle = new List<PresupuestoDetalle>();
    }

    public double MontoPresupuesto()
    {
        int monto = detalle.Sum(d => d.Cantidad * d.Producto.Precio);
        return monto;
    }
    public double MontoPresupuestoConIva()
    {
        return MontoPresupuesto() * 1.21;
    }
    public int CantidadProductos()
    {
        return detalle.Sum(d => d.Cantidad);
    }


}