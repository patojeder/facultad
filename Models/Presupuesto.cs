using Microsoft.AspNetCore.SignalR.Protocol;

public class Presupuesto
{
    private int idPresupuesto;
    private string nombreDestinatario;
    private DateTime fechaCreacion;
    private List<PresupuestoDetalle> detalle;

    public Presupuesto(){}
    public Presupuesto(int id, string destinatario)
    {
        IdPresupuesto = id;
        NombreDestinatario = destinatario;
        Detalle = new List<PresupuestoDetalle>();
    }

    public int IdPresupuesto { get => idPresupuesto; set => idPresupuesto = value; }
    public string NombreDestinatario { get => nombreDestinatario; set => nombreDestinatario = value; }
    public List<PresupuestoDetalle> Detalle { get => detalle; set => detalle = value; }
    public DateTime FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }

    public int  MontoPresupuesto()
    {
        int total = Detalle.Sum(d => d.Cantidad * d.Producto.Precio);
        /*foreach (var item in Detalle)
        {
            total += item.Cantidad * item.Producto.Precio;
        }*/   
        return total;   
    }

    public double montoPresupuestoConIva()
    {
        int cantSinIva = MontoPresupuesto();
        return cantSinIva * 1.21;
    }

    public int CantidadProductos()
    {
        return Detalle.Sum(d => d.Cantidad);
    }
}