public interface IUserRepository
{
    Usuario ObtenerUsuario(string username, string password);
}