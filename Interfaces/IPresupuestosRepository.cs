public interface IPresupuestosRepository
{
    void CrearPresupuesto(Presupuesto presupuesto);
    List<Presupuesto> GetPresupuestos();
    List<PresupuestoDetalle> ObtenerDetalle(int id);
}