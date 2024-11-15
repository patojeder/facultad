public interface IPresupuestosRepository
{
    void CrearPresupuesto(Presupuesto presupuesto);
    List<Presupuesto> GetPresupuestos();
    List<PresupuestoDetalle> ObtenerDetalle(int id);
    Presupuesto ObtenerPresupuestoPorId(int id);
    void EliminarPresupuestoPorId(int idPresupuesto);
    void ModificarPresupuestoQ(Presupuesto presupuesto);
}