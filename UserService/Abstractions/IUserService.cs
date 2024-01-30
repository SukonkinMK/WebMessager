using UserService.Models;

namespace UserService.Abstractions
{
    public interface IUserService
    {
        public int UserAdd(LoginForm form, int roleId);
        public int UserDel(int userId);
        public int CheckUserRole(LoginForm form);
        public IEnumerable<UserModel> GetUsers();
    }
}
