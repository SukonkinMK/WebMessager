using AutoMapper;
using UserService.Abstractions;
using UserService.Models;

namespace UserService.Services
{
    public class UserService : IUserService
    {
        private readonly UserDbContext _context;
        private readonly IMapper _mapper;

        public UserService(UserDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public int CheckUserRole(LoginForm form)
        {
            using (_context)
            {
                    var user = _context.Users.FirstOrDefault(x => x.Login.ToLower().Equals(form.Email.ToLower()));
                if (user is null)
                    return -1;
                if (user.Password.SequenceEqual(form.Password))
                {
                    return user.RoleId;
                }
                else
                    return -2;
            }
        }

        public int UserAdd(LoginForm form, int roleId)
        {
            
            using (_context)
            {
                //добавление ролей при их отсутствии
                if (!_context.Roles.Any(x => x.Id == 1))
                {
                    var role = new Role { Name = "admin" };
                    _context.Roles.Add(role);
                    _context.SaveChanges();
                }
                if (!_context.Roles.Any(x => x.Id == 2))
                {
                    var role = new Role { Name = "user" };
                    _context.Roles.Add(role);
                    _context.SaveChanges();
                }
                if (roleId == 1)
                {
                    var adminCount = _context.Users.Count(x => x.RoleId == 1);
                    if (adminCount > 0)
                        return -1; //Администратор может быть только один
                }
                //проверка существования пользователя
                var userExist = _context.Users.FirstOrDefault(x => x.Login.ToLower().Equals(form.Email.ToLower()));
                UserEntity entity = null;
                if (userExist != null)
                {
                    return -2; //Пользователь уже существует
                }
                entity = new UserEntity
                {
                    Login = form.Email,
                    Password = form.Password,
                    RoleId = roleId
                };
                _context.Users.Add(entity);
                _context.SaveChanges();
                return entity.Id;
            }
        }

        public IEnumerable<UserModel> GetUsers()
        {
            using (_context)
            {
                var users = _context.Users.Select(x => _mapper.Map<UserModel>(x)).ToList();

                return users;
            }
        }

        public int UserDel(int userId)
        {
            using (_context)
            {
                var user = _context.Users.FirstOrDefault(x => x.Id == userId);
                if (user != null)
                {
                    if (user.RoleId == 1)
                        return -2;

                    _context.Users.Remove(user);
                    _context.SaveChanges();
                    return userId;
                }
                return -1;
            }
        }     
    }
}
