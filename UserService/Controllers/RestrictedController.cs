using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.Abstractions;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestrictedController : ControllerBase
    {
        private readonly IUserService _userService;

        public RestrictedController(IUserService userService)
        {
            _userService = userService;
        }

        //Добавить администратора (первый пользователь в системе)
        [AllowAnonymous]
        [HttpPost]
        [Route("add_admin")]
        public ActionResult AddAdmin([FromBody] LoginForm form)
        {
            int res = _userService.UserAdd(form, 1);
            if (res == -1)
                return BadRequest("Администратор может быть только один");
            else if (res == -2)
                return BadRequest("Пользовательс таким Email уже существует");
            else
                return Ok(res);
        }

        //Добавить пользователя (обязательна проверка на дублирующиеся имена/адреса)
        [HttpPost]
        [Route("add_user")]
        [Authorize(Roles = "1")]
        public ActionResult AddUser([FromBody] LoginForm form)
        {
            var res = _userService.UserAdd(form, 2);
            if (res == -2)
                return BadRequest("Пользовательс таким Email уже существует");
            else
                return Ok(res);
        }

        //Получить список пользователей
        [HttpGet]
        [Route("get_users")]
        [Authorize(Roles = "1, 2")]
        public ActionResult GetUsers()
        {
            var response = _userService.GetUsers();
            return Ok(response);
        }

        // Удалить пользователя (доступ только у администратора), администратор не может удалить сам себя
        [HttpDelete]
        [Route("del_user")]
        [Authorize(Roles = "1")]
        public ActionResult DelUser([FromBody] int userId)
        {
            var res = _userService.UserDel(userId);
            if (res == -1)
                return NotFound();
            else if (res == -2)
                return BadRequest("Администратор не может удалить сам себя");
            else
                return Ok(res);
        }

        //Метод возвращающий ID пользователя при обращении с JWT-токеном
        [HttpGet]
        [Route("check_user_auth")]
        [Authorize(Roles = "1, 2")]
        public IActionResult CheckUserAuth()
        {
            var currentUser = GetCurrentUser();
            return Ok(currentUser.UserId);
        }
        private UserModel GetCurrentUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;
                return new UserModel
                {
                    Email = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value,
                    RoleId = int.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value),
                    UserId = int.Parse(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value)
                };
            }
            return null;
        }
    }
}
