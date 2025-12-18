using siapv_backend.Models;
namespace siapv_backend.Models
{
    public class userRole
    {
        public int Id { get; set; }
        public int personaId { get; set; }
        public int empleadoId { get; set; }
        public int userId {get; set;}
        public int roleId { get; set; }
        public Role? role { get; set; }

    }
}