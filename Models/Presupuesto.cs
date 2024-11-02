using System.Text.Json.Serialization;

public class Presupuesto
{
    private static int autoincremento = 5;
    private int idPresupuesto;
    private string nombreDestinatario;
    private string fechaCreacion;
    private List<PresupuestoDetalle> detalle;


    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public string FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }

    public Presupuesto(int idPresupuesto, string nombreDestinatario, string fechaCreacion)
    {
        this.idPresupuesto = idPresupuesto;
        this.nombreDestinatario = nombreDestinatario;
        this.fechaCreacion = fechaCreacion;
        detalle = new List<PresupuestoDetalle>();
    }

    public Presupuesto()
    {
        IdPresupuesto = autoincremento++;
        FechaCreacion = (DateTime.Today).ToString();
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