
using Microsoft.Data.Sqlite;

class PresupuestoRepository
{
    private string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

    public bool CrearPresupuesto(Presupuesto presupuesto)
    {
        ProductoRepository repoProductos = new ProductoRepository();

        foreach (var detalle in presupuesto.Detalle)
        {
            if (repoProductos.ObtenerProducto(detalle.Producto.IdProducto) == null)
            {
                return false;
            }
        }

        string query = @"INSERT INTO Presupuestos (NombreDestinatario, FechaCreacion) 
        VALUES (@destinatario, @fecha)";

        string query2 = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) 
        VALUES (@idP, @idPr, @cant)";

        string query3 = @"SELECT MAX(idPresupuesto) AS idMax FROM Presupuestos;";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresupuesto", presupuesto.IdPresupuesto);
            command.Parameters.AddWithValue("@destinatario", presupuesto.NombreDestinatario);
            command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
            command.ExecuteNonQuery();
            SqliteCommand command3 = new SqliteCommand(query3, connection);
            using (SqliteDataReader reader = command3.ExecuteReader())
            {
                if (reader.Read())
                {
                    foreach (var detalle in presupuesto.Detalle)
                    {
                        SqliteCommand command2 = new SqliteCommand(query2, connection);
                        command2.Parameters.AddWithValue("@idP", Convert.ToInt32(reader["idMax"]));
                        command2.Parameters.AddWithValue("@idPr", detalle.Producto.IdProducto);
                        command2.Parameters.AddWithValue("@cant", detalle.Cantidad);
                        command2.ExecuteNonQuery();
                    }
                }
            }
            connection.Close();
        }
        return true;
    }

    public List<Presupuesto> ObtenerPresupuestos()
    {
        List<Presupuesto> presupuestos = new List<Presupuesto>();

        string query = 
        @"SELECT 
            P.idPresupuesto,
            P.NombreDestinatario,
            P.FechaCreacion,
            PR.idProducto,
            PR.Descripcion AS Producto,
            PR.Precio,
            PD.Cantidad
        FROM 
            Presupuestos P
        JOIN 
            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
        JOIN 
            Productos PR ON PD.idProducto = PR.idProducto
        ORDER BY P.idPresupuesto;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                int id = -1;
                Presupuesto ultPresupuesto = new Presupuesto(-1, " ", DateTime.Now);
                while (reader.Read())
                {
                    if (id == -1 || id != Convert.ToInt32(reader["idPresupuesto"]))
                    {
                        if (id != -1) presupuestos.Add(ultPresupuesto);
                        ultPresupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
                    PresupuestoDetalle detalle = new PresupuestoDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                    ultPresupuesto.Detalle.Add(detalle);
                    id = Convert.ToInt32(reader["idPresupuesto"]);
                }
                presupuestos.Add(ultPresupuesto);

            }
            connection.Close();
        }
        return presupuestos;
    }

    public Presupuesto ObtenerPresupuestoPorId(int id)
    {
        Presupuesto presupuesto = null;

        string query = 
        @"SELECT 
            P.idPresupuesto,
            P.NombreDestinatario,
            P.FechaCreacion,
            PR.idProducto,
            PR.Descripcion AS Producto,
            PR.Precio,
            PD.Cantidad
        FROM 
            Presupuestos P
        JOIN 
            PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
        JOIN 
            Productos PR ON PD.idProducto = PR.idProducto
        WHERE 
            P.idPresupuesto = @id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);
            int cont = 1;
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    if (cont == 1)
                    {
                        presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
                    }
                    Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
                    PresupuestoDetalle detalle = new PresupuestoDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
                    presupuesto.Detalle.Add(detalle);
                    cont++;
                }
            }
            connection.Close();
        }
        return presupuesto;
    }

    public bool AgregarProducto(int idPresupuesto, int idProducto, int cantidad)
    {
        ProductoRepository repoProductos = new ProductoRepository();
        if (ObtenerPresupuestoPorId(idPresupuesto) == null || repoProductos.ObtenerProducto(idProducto) == null)
        {
            return false;
        }


        string query = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPresu, @idProd, @cant)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@idPresu", idPresupuesto);
            command.Parameters.AddWithValue("@idProd", idProducto);
            command.Parameters.AddWithValue("@cant", cantidad);
            command.ExecuteNonQuery();
            connection.Close();
        }
        return true;
    }

    public void EliminarPresupuestoPorId(int idPresupuesto)
    {


        string query = @"DELETE FROM Presupuestos WHERE idPresupuesto = @IdP;";
        string query2 = @"DELETE FROM PresupuestosDetalle WHERE idPresupuesto = @Id;";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            SqliteCommand command2 = new SqliteCommand(query2, connection);
            command.Parameters.AddWithValue("@IdP", idPresupuesto);
            command2.Parameters.AddWithValue("@Id", idPresupuesto);
            command2.ExecuteNonQuery();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }


}