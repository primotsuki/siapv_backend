using siapv_backend.DB;
using siapv_backend.Models.DTORequests;
using siapv_backend.Models.DTOResponses;
using siapv_backend.Models.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;
using siapv_backend.Models;

namespace siapv_backend.Services
{
    public class UserService: IUserService {
        private readonly AppSettings _appSettings;
        private readonly LdapService _ldapService;
        private readonly DBUsuariosContext _dbUsers;
        private readonly AppDbContext _db;
        public UserService(IOptions<AppSettings> appSettings, LdapService ldapService, DBUsuariosContext dbUsers, AppDbContext db)
        {
            _appSettings = appSettings.Value;

            _ldapService = ldapService;
            _dbUsers = dbUsers;
            _db = db;
        }
        public async Task<AuthenticateResponse?> Authenticate(LdapRequest user)
        {
            var isAuthenticated = _ldapService.AuthenticateAsync(user.Username, user.Password);
            if (!isAuthenticated) return null;
            var user_unique = await (from u in _dbUsers.Usuarios
                                     join p in _dbUsers.Personas on u.PersonaId equals p.Id
                                     where u.username == user.Username
                                     select new AuthenticateResponse
                                     {
                                         userId = u.Id,
                                         personaId = p.Id,
                                         nombres = $"{p.Nombres} {p.apellido_paterno} {p.apellido_materno}",
                                         Username = u.username,
                                         email = u.email,
                                     }).FirstOrDefaultAsync();
            
            if (user_unique == null) return null;
            var token = await generateJwtToken(user_unique);
            var role = await (from r in _db.roles
                            join ur in _db.userRoles on r.Id equals ur.roleId
                            where ur.userId == user_unique.userId
                            select new
                            {
                                ur.userId,
                                r.Id,
                                ur.personaId,
                                r.role
                            }).FirstOrDefaultAsync();
            user_unique.Token = token;
            user_unique.roleId = role?.Id ?? 6;
            user_unique.role = role?.role ?? "usuario";
            return user_unique;
        }
        private async Task<string> generateJwtToken(AuthenticateResponse user)
        {
            //Generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = await Task.Run(() =>
            {

                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.userId.ToString()) }),
                    Expires = DateTime.UtcNow.AddDays(7),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                return tokenHandler.CreateToken(tokenDescriptor);
            });

            return tokenHandler.WriteToken(token);
        }
        public async Task<DTOUserContext?> GetUserContextById(int id)
        {
            var query = await (from u in _dbUsers.Usuarios
                           join p in _dbUsers.Personas on u.PersonaId equals p.Id                        
                           where u.Id == id
                           select new DTOUserContext
                           {
                               userId = u.Id,
                               personaId = p.Id,
                               username = u.username,
                               sexo = p.sexo
                           }).FirstOrDefaultAsync();
            return query;
        }
    }
}