using Microsoft.AspNetCore.Mvc;

public class LoginController: Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUserRepository _userRepository;
    
    public LoginController(ILogger<LoginController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public IActionResult Login(LoginViewModel model)
    {
        //Verificar que los datos de entrada no esten vacios
        if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
        {
            model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
            return View("Index", model);
        }

        //Si el usuario existe y las credenciales son correctas
        Usuario user = _userRepository.ObtenerUsuario(model.Username, model.Password);
        if(user != null)
        {
            //Redirigir a la pagina principal o dashboard
            HttpContext.Session.SetString("IsAuthenticated", "True");
            HttpContext.Session.SetString("User", user.Username);
            HttpContext.Session.SetString("Rol", user.Rol.ToString());

            return RedirectToAction("Index", "Productos");
        }

        //Si las credenciales no son correctas, mostrar mensaje de error
        model.ErrorMessage = "Credenciales inválidas";
        model.IsAuthenticated = false;
        return View("Index", model);
    }

    public IActionResult Index()
    {
        var model = new LoginViewModel
        {
            IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "True",
        };
        return View(model);
    }

    public IActionResult Logout()
    {
        // Limpiar la sesión
        HttpContext.Session.Clear();

        // Redirigir a la vista de login
        return RedirectToAction("Index");
    }
}