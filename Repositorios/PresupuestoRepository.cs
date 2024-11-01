using Microsoft.Data.Sqlite;

public class PresupuestoRepository
{
    private string cadenaConnection = "Data Source=db/Tienda.db;Cache=Shared";

    private ProductoRepository _productoRepository;

    // Constructor que recibe ProductoRepository como dependencia
    public PresupuestoRepository(ProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    //Es mejor pasar ProductoRepository como una dependencia en el constructor de PresupuestoRepository para facilitar la reutilización y facilitar pruebas.

    // 1. Crear Presupuesto con detalles
    public void CrearPresupuesto(Presupuesto presupuesto)
    {
        var queryPresupuesto = "INSERT INTO Presupuestos(IdPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@id, @destinatario, @fecha)";

        using (SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            connection.Open();

            // Insertar el presupuesto en la tabla Presupuestos
            var command = new SqliteCommand(queryPresupuesto, connection);
            command.Parameters.Add(new SqliteParameter("@id", presupuesto.IdPresupuesto));
            command.Parameters.Add(new SqliteParameter("@destinatario", presupuesto.NombreDestinatario));
            command.Parameters.Add(new SqliteParameter("@fecha", presupuesto.FechaCreacion));

            command.ExecuteNonQuery();

            // Insertar cada detalle en PresupuestosDetalle
            var queryDetalle = "INSERT INTO PresupuestosDetalle(idPresupuesto, idProducto, Cantidad) VALUES (@idPresupuesto, @idProducto, @cantidad)";

            foreach (var detalle in presupuesto.Detalle)
            {
                var commandDetalle = new SqliteCommand(queryDetalle, connection);
                commandDetalle.Parameters.Add(new SqliteParameter("@idPresupuesto", presupuesto.IdPresupuesto));
                commandDetalle.Parameters.Add(new SqliteParameter("@idProducto", detalle.Producto.IdProducto));
                commandDetalle.Parameters.Add(new SqliteParameter("@cantidad", detalle.Cantidad));

                commandDetalle.ExecuteNonQuery();
            }

            connection.Close();
        }
    }

    public List<Presupuesto> ListarPresupuestos()
    {
        var presupuestos = new List<Presupuesto>();
        var query = "SELECT IdPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";

        using (SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            connection.Open();
            var command = new SqliteCommand(query, connection);

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var presupuesto = new Presupuesto();

                    presupuesto.IdPresupuesto = reader.GetInt32(0);
                    presupuesto.NombreDestinatario = reader.GetString(1);
                    presupuesto.FechaCreacion = reader.GetDateTime(2);
                    presupuesto.Detalle = ObtenerDetallesPresupuesto(reader.GetInt32(0));

                    presupuestos.Add(presupuesto);
                }
            }
            connection.Close();
        }
        return presupuestos;
    }

    public List<PresupuestoDetalle> ObtenerDetallesPresupuesto(int idPresupuesto)
    {
        var detalles = new List<PresupuestoDetalle>();

        var query = "SELECT idProducto, Cantidad FROM PresupuestosDetalle WHERE idPresupuesto = @id";

        using (SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            connection.Open();
            var command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", idPresupuesto));

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    PresupuestoDetalle detalle = new PresupuestoDetalle();
                    {
                        detalle.Producto = _productoRepository.ObtenerProducto(reader.GetInt32(0));
                        detalle.Cantidad = reader.GetInt32(1);
                    }
                    detalles.Add(detalle);
                }
            }
            connection.Close();
        }
        return detalles;
    }

    // 4. Eliminar un Presupuesto por ID
    public void EliminarPresupuesto(int idPresupuesto)
    {
        using (SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            connection.Open();

            // Eliminar detalles del presupuesto primero
            var queryDetalles = "DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @id";
            var commandDetalles = new SqliteCommand(queryDetalles, connection);
            commandDetalles.Parameters.Add(new SqliteParameter("@id", idPresupuesto));
            commandDetalles.ExecuteNonQuery();

            // Luego eliminar el presupuesto
            var queryPresupuesto = "DELETE FROM Presupuestos WHERE IdPresupuesto = @id";
            var commandPresupuesto = new SqliteCommand(queryPresupuesto, connection);
            commandPresupuesto.Parameters.Add(new SqliteParameter("@id", idPresupuesto));
            commandPresupuesto.ExecuteNonQuery();

            connection.Close();
        }
    }
}
