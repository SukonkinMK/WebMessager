using UserService.Models;

namespace UserService.Abstractions
{
    public interface IUserAuthenticationService
    {
        public string Authenticate(LoginForm form);
    }
}
