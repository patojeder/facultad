public interface IProductosRepository
{
    void CrearProducto(Producto producto);
    List<Producto> ObtenerProductos();
    void ModificarProducto(int id, Producto producto);
    Producto ObtenerProducto(int id);
    void EliminarProducto(int id);
}