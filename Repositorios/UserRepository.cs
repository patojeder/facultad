using Microsoft.Data.Sqlite;

public class UserRepository: IUserRepository
{
    //private readonly List<Usuario> _usuarios;
    private string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

    // public UserRepository()
    // {
    //     _usuarios = new List<Usuario>
    //     {
    //         new Usuario{Id = 1, Username = "admin", Password = "admin123", Rol = Rol.Admin},
    //         new Usuario{Id = 2, Username = "cliente", Password = "cliente123", Rol = Rol.Cliente}
    //     };
    // }

    // public Usuario ObtenerUsuario(string username, string password)
    // {
    //     return _usuarios.Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(password, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    // }

    public void CrearUsuario(Usuario user)
    {
        string query = @"INSERT INTO Usuarios (Nombre, Usuario, Contraseña, Rol) VALUES (@nombre, @user, @password, @rol)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", user.Nombre);
            command.Parameters.AddWithValue("@user", user.Username);
            command.Parameters.AddWithValue("@password", user.Password);
            command.Parameters.AddWithValue("@rol", user.Rol);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    public Usuario ObtenerUsuario(string usuario, string password)
    {

        var userEncontrado = new Usuario();
        string query = @"SELECT * FROM Usuarios WHERE Usuario = @user AND Contraseña = @password ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@user", usuario);
            command.Parameters.AddWithValue("@password", password);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    userEncontrado.Id = Convert.ToInt32(reader["idUser"]);
                    userEncontrado.Nombre = reader["Nombre"].ToString();
                    userEncontrado.Username = reader["Usuario"].ToString();
                    userEncontrado.Password = reader["Contraseña"].ToString();
                    int valorRol = Convert.ToInt32(reader["idUser"]);
                    userEncontrado.Rol = (Rol)valorRol;
                }
            }
            connection.Close();
        }
        return userEncontrado;
    }

    public void EliminarUsuario(int id)
    {
        string query = @"DELETE FROM Usuario WHERE idUser = @Id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            connection.Close();
        }
    }
}