using siapv_backend.Models;
using siapv_backend.Models.DTORequests;
using siapv_backend.Models.DTOResponses;

namespace siapv_backend.Services
{
    public interface IUserService
    {
        Task<AuthenticateResponse?> Authenticate(LdapRequest model);
        Task<DTOUserContext?> GetUserContextById(int id);
    }
}