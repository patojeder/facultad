using Microsoft.Data.Sqlite;

class PresupuestosRepository
{
    string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";
    public void CrearPresupuesto(Presupuesto presupuesto)
    {
        string query1 = @"INSERT INTO Presupuestos (idPresupuesto, NombreDestinatario, FechaCreacion) VALUES (@idPre, @nombrePre, @fechaPre)";
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (SqliteCommand command = new SqliteCommand(query1, connection))
            {
                command.Parameters.Add(new SqliteParameter("@idPre", presupuesto.IdPresupuesto));
                command.Parameters.Add(new SqliteParameter("@nombrePre", presupuesto.NombreDestinatario));
                command.Parameters.Add(new SqliteParameter("@fechaPre", presupuesto.FechaCreacion));

                command.ExecuteNonQuery();
            }
        }

        foreach (var item in presupuesto.Detalle)
        {
            string query2 = @"INSERT INTO PresupuestosDetalle (idPresupuesto, idProducto, Cantidad) VALUES (@idPre, @idProdu, @canti)";
            using (SqliteConnection connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                using (SqliteCommand command = new SqliteCommand(query2, connection))
                {
                    command.Parameters.Add(new SqliteParameter("@idPre", presupuesto.IdPresupuesto));
                    command.Parameters.Add(new SqliteParameter("@idProdu", item.Producto.IdProducto));
                    command.Parameters.Add(new SqliteParameter("@canti", item.Cantidad));

                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public List<Presupuesto> GetPresupuestos()
    {
        string queryDetalle = @"SELECT idPresupuesto, NombreDestinatario, FechaCreacion FROM Presupuestos";
        List<Presupuesto> presupuestos = new List<Presupuesto>();

        using (SqliteConnection sqlitecon = new SqliteConnection(connectionString))
        {
            sqlitecon.Open();
            using (SqliteCommand command = new SqliteCommand(queryDetalle, sqlitecon))
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int idpres = Convert.ToInt32(reader["idPresupuesto"]);
                    string nombre = Convert.ToString(reader["NombreDestinatario"]);
                    string fecha = Convert.ToString(reader["FechaCreacion"]);
                    Presupuesto presu1 = new Presupuesto(idpres, nombre, fecha); 
                    presupuestos.Add(presu1);
                }
            }
        }
        return presupuestos;
    }

    public List<PresupuestoDetalle> ObtenerDetalle(int id)
    {
        string query = @"SELECT p.idProducto, p.Descripcion, p.Precio, pd.Cantidad 
                         FROM Productos AS p
                         INNER JOIN PresupuestosDetalle AS pd USING (idProducto)
                         WHERE pd.idPresupuesto = @idquery";

        List<PresupuestoDetalle> lista = new List<PresupuestoDetalle>();
        
        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            using (SqliteCommand command = new SqliteCommand(query, connection))
            {
                command.Parameters.Add(new SqliteParameter("@idquery", id));
                
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        PresupuestoDetalle Pd = new PresupuestoDetalle();
                        Producto nuevoProducto = new Producto();

                        nuevoProducto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                        nuevoProducto.Descripcion = Convert.ToString(reader["Descripcion"]);
                        nuevoProducto.Precio = Convert.ToInt32(reader["Precio"]);
                        Pd.Cantidad = Convert.ToInt32(reader["Cantidad"]);

                        Pd.Producto = nuevoProducto;
                        
                        lista.Add(Pd);
                    }
                }
            }
        }
        return lista;
    }

    public Presupuesto ObtenerPresupuestoPorId(int id)
    {
        Presupuesto presupuesto = null;

        string query = @"SELECT 
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
                        presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToString(reader["FechaCreacion"]));
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

    // public List<Presupuesto> ObtenerPresupuestos()
    // {
    //     List<Presupuesto> presupuestos = new List<Presupuesto>();

    //     string query = @"SELECT 
    //         P.idPresupuesto,
    //         P.NombreDestinatario,
    //         P.FechaCreacion,
    //         PR.idProducto,
    //         PR.Descripcion AS Producto,
    //         PR.Precio,
    //         PD.Cantidad
    //     FROM 
    //         Presupuestos P
    //     JOIN 
    //         PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
    //     JOIN 
    //         Productos PR ON PD.idProducto = PR.idProducto
    //     ORDER BY P.idPresupuesto;";

    //     using (SqliteConnection connection = new SqliteConnection(connectionString))
    //     {
    //         connection.Open();
    //         SqliteCommand command = new SqliteCommand(query, connection);

    //         using (SqliteDataReader reader = command.ExecuteReader())
    //         {
    //             int id = -1;
    //             Presupuesto ultPresupuesto = new Presupuesto(-1, " ", DateTime.Now);
    //             while (reader.Read())
    //             {
    //                 if (id == -1 || id != Convert.ToInt32(reader["idPresupuesto"]))
    //                 {
    //                     if (id != -1) presupuestos.Add(ultPresupuesto);
    //                     ultPresupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
    //                 }
    //                 Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
    //                 PresupuestoDetalle detalle = new PresupuestoDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
    //                 ultPresupuesto.Detalle.Add(detalle);
    //                 id = Convert.ToInt32(reader["idPresupuesto"]);
    //             }
    //             presupuestos.Add(ultPresupuesto);

    //         }
    //         connection.Close();
    //     }
    //     return presupuestos;
    // }

    // public Presupuesto ObtenerPresupuestoPorId(int id)
    // {
    //     Presupuesto presupuesto = null;

    //     string query = @"SELECT 
    //         P.idPresupuesto,
    //         P.NombreDestinatario,
    //         P.FechaCreacion,
    //         PR.idProducto,
    //         PR.Descripcion AS Producto,
    //         PR.Precio,
    //         PD.Cantidad
    //     FROM 
    //         Presupuestos P
    //     JOIN 
    //         PresupuestosDetalle PD ON P.idPresupuesto = PD.idPresupuesto
    //     JOIN 
    //         Productos PR ON PD.idProducto = PR.idProducto
    //     WHERE 
    //         P.idPresupuesto = @id;";

    //     using (SqliteConnection connection = new SqliteConnection(connectionString))
    //     {
    //         connection.Open();
    //         SqliteCommand command = new SqliteCommand(query, connection);
    //         command.Parameters.AddWithValue("@id", id);
    //         int cont = 1;
    //         using (SqliteDataReader reader = command.ExecuteReader())
    //         {
    //             while (reader.Read())
    //             {
    //                 if (cont == 1)
    //                 {
    //                     presupuesto = new Presupuesto(Convert.ToInt32(reader["idPresupuesto"]), reader["NombreDestinatario"].ToString(), Convert.ToDateTime(reader["FechaCreacion"]));
    //                 }
    //                 Producto producto = new Producto(Convert.ToInt32(reader["idProducto"]), reader["Producto"].ToString(), Convert.ToInt32(reader["Precio"]));
    //                 PresupuestoDetalle detalle = new PresupuestoDetalle(producto, Convert.ToInt32(reader["Cantidad"]));
    //                 presupuesto.Detalle.Add(detalle);
    //                 cont++;
    //             }
    //         }
    //         connection.Close();
    //     }
    //     return presupuesto;
    // }

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

    public void ModificarPresupuesto(Presupuesto presupuesto)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            // Primero, actualizamos el presupuesto en la tabla Presupuestos
            string query = @"UPDATE Presupuestos 
                         SET NombreDestinatario = @destinatario, FechaCreacion = @fecha
                         WHERE idPresupuesto = @id";

            using (var command = new SqliteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@destinatario", presupuesto.NombreDestinatario);
                command.Parameters.AddWithValue("@fecha", presupuesto.FechaCreacion);
                command.Parameters.AddWithValue("@id", presupuesto.IdPresupuesto);
                command.ExecuteNonQuery();
            }

            //No pude modificar los detalles
            connection.Close();
        }
    }


}