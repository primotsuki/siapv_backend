

namespace siapv_backend.Models.DTOResponses
{
    public class AuthenticateResponse
    {
        public int userId { get; set; }
        public int personaId { get; set; }
        public int roleId {  get; set; }
        public string role { get; set; } = null!;
        public string nombres { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string email { get; set; } = null!;
        public string Token { get; set; } = null!;
    }

    public class PermissionsAssigned
    {
        public int permissionId { get; set; }
        public string descripcion { get; set; } = null!;
    }
}