public class LoginViewModel
{
    public int Id {get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ErrorMessage { get; set; }
    public bool IsAuthenticated { get; set; } 
} 