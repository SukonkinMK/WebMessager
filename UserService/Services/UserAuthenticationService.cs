using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using RSALib;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using UserService.Abstractions;
using UserService.Models;

namespace UserService.Services
{
    public class UserAuthenticationService : IUserAuthenticationService
    {
        private readonly UserDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public UserAuthenticationService(UserDbContext context, IConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        public string Authenticate(LoginForm form)
        {
            using (_context)
            {
                var entity = _context.Users
                    .FirstOrDefault(
                    x => x.Login.ToLower().Equals(form.Email.ToLower()) &&
                    x.Password.Equals(form.Password));

                if (entity == null)
                    return "";

                var user = _mapper.Map<UserModel>(entity);

                return GenerateToken(user);
            }
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new RsaSecurityKey(RSATools.GetRSAPrivateKey());
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256Signature);
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
