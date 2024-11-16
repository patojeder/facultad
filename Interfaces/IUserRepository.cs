public interface IUserRepository
{
    void CrearUsuario(Usuario user);
    Usuario ObtenerUsuario(string user, string password);
    void EliminarUsuario(int id);
}