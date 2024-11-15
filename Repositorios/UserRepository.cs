public class UserRepository: IUserRepository
{
    private readonly List<Usuario> _usuarios;

    public UserRepository()
    {
        _usuarios = new List<Usuario>
        {
            new Usuario{Id = 1, Username = "admin", Password = "admin123", AccessLevel = AccessLevel.Admin},
            new Usuario{Id = 2, Username = "invitado", Password = "invitado123", AccessLevel = AccessLevel.Invitado}
        };
    }

    public Usuario ObtenerUsuario(string username, string password)
    {
        return _usuarios.Where(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && u.Password.Equals(password, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
    }
}