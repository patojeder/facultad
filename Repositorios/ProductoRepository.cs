using Microsoft.Data.Sqlite;

public class ProductoRepository
{

    private string cadenaConnection = "Data Source=db/Tienda.db;Cache=Shared";

    public void CrearProducto(Producto producto)
    {
        var query = $"INSERT INTO Productos (idProducto, Descripcion, Precio) VALUES (@idProducto, @descripcion, @precio)";
        using (SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            connection.Open();

            var command = new SqliteCommand(query, connection);

            command.Parameters.Add(new SqliteParameter(@"idProducto", producto.IdProducto));
            command.Parameters.Add(new SqliteParameter(@"descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter(@"precio", producto.Precio));

            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public void ModificarProducto(int id, Producto producto)
    {
        var query = $"UPDATE Productos SET Descripcion = @descripcion, Precio = @precio WHERE idProducto = @idProducto";
        using (SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            connection.Open();

            var command = new SqliteCommand(query, connection);

            // Agrega los parámetros
            command.Parameters.Add(new SqliteParameter(@"descripcion", producto.Descripcion));
            command.Parameters.Add(new SqliteParameter(@"precio", producto.Precio));
            command.Parameters.Add(new SqliteParameter(@"idProducto", id));

            // Ejecuta el comando
            command.ExecuteNonQuery();

            connection.Close();
        }
    }

    public List<Producto> ListarProductos()
    {
        var query = "SELECT * FROM Productos";
        List<Producto> productos = new List<Producto>();
        using (SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);

            using(var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    Producto producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                    productos.Add(producto);
                }
            }
            connection.Close();
        }
        return productos;
    }

    public Producto ObtenerProducto(int id)
    {
        var query = "SELECT * FROM Productos WHERE idProducto = @id";
        Producto producto = null; //Lo inicializo en null para manejar los casos donde no lo encuentra

        using(SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));

            connection.Open();
            using(var reader = command.ExecuteReader())
            {
                if(reader.Read()) //Si encuentra un producto
                {
                    producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Descripcion"].ToString(), Convert.ToInt32(reader["Precio"]));
                }
            }

            connection.Close();
        }
        return producto; //Retorna null si no lo encontro o el producto encontrado.
    }

    public void EliminarProducto(int id)
    {
        var query = "DELETE FROM Productos WHERE idProducto = @id";
        using(SqliteConnection connection = new SqliteConnection(cadenaConnection))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.Add(new SqliteParameter("@id", id));

            command.ExecuteNonQuery();
            //No se si debo mostrar algun mensaje o algo dependiendo como resulte el executeNonQuery. Preguntar
            connection.Close();
        }
    }

    

}