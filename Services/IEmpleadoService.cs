using siapv_backend.Models.DTOResponses;
using siapv_backend.Models.DTORequests;
using siapv_backend.Models;
namespace siapv_backend.Services
{
    public interface IEmpleadoService
    {
        Task<List<DTOEmpleadoInfo>> getempleadosByDependencia(int id);
        Task<EmpleadosContrato?> getEmpleadoActivoByPersonaId(int? personaId);
        Task<List<DTOUserInfo>> getActiveUsers(DTOUserParams request);
        Task<List<Role>> getRoles();
        Task<userRole> updateUserRole(DTOUserRole request);
    }
}