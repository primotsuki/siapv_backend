using siapv_backend.DB;
using siapv_backend.Models.DTOResponses;
using siapv_backend.Models.DTORequests;
using siapv_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace siapv_backend.Services
{
    public class EmpleadoService: IEmpleadoService
    {
        public readonly DBUsuariosContext userDb;
        public readonly AppDbContext _db;
        public EmpleadoService(DBUsuariosContext _usdb, AppDbContext db)
        {
            userDb = _usdb;
            _db = db;
        }
        public async Task<List<DTOEmpleadoInfo>> getempleadosByDependencia(int dependenciaId)
        {
            var query = await (from e in userDb.EmpleadosContratos
                join p in userDb.Personas on e.personaId equals p.Id
                where e.DependenciaId == dependenciaId
                && e.activo select new DTOEmpleadoInfo
                {
                    empleadoId = e.Id,
                    nombres = p.Nombres,
                    apellido_paterno = p.apellido_paterno,
                    apellido_materno = p.apellido_materno,
                    carnet = p.carnet,
                    denominacionCargo = e.DenominacionCargo
                }).ToListAsync();
            return query;
        }
        public async Task<List<Role>> getRoles()
        {
            return await _db.roles.ToListAsync();
        }
        public async Task<EmpleadosContrato?> getEmpleadoActivoByPersonaId(int? personaId)
        {
            var empleado = await userDb.EmpleadosContratos.FirstOrDefaultAsync(x => x.personaId == personaId && x.activo == true);
            return empleado;
        }
        public async Task<List<DTOUserInfo>> getActiveUsers(DTOUserParams request)
        {
            var query = from u in userDb.Usuarios
                         join p in userDb.Personas
                         on u.PersonaId equals p.Id
                         select new DTOUserInfo
                         {
                             userId = u.Id,
                             personaId = u.PersonaId,
                             nombres = p.Nombres,
                             apellido_paterno = p.apellido_paterno,
                             apellido_materno = p.apellido_materno,
                             SearchVector = p.SearchVector,

                         };
            List<DTOUserInfo> result;
          if (request.searchTerm != null && request.searchTerm.Length > 3)
            {
                result =  query.Where(p => p.SearchVector
                .Matches(EF.Functions.PlainToTsQuery("spanish", request.searchTerm)))
                .Skip((request.page - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToList();
            }
            else
            {
                result = query.
                    Skip((request.page - 1) * request.pageSize)
                    .Take(request.pageSize)
                    .ToList ();
            }
            var roledb = (from r in _db.roles
                            join ur in _db.userRoles on r.Id equals ur.roleId
                            select new
                            {
                                ur.userId,
                                r.Id,
                                ur.personaId,
                                r.role
                            }).ToList();
            
            var response = (from pe in result
                            join ro in roledb on pe.userId equals ro.userId
                            into rol from ro in rol.DefaultIfEmpty() 
                            select new DTOUserInfo
                            {
                                userId = pe.userId,
                                personaId = pe.personaId,
                                empleadoId = pe.empleadoId,
                                nombres = pe.nombres,
                                apellido_paterno = pe.apellido_paterno,
                                apellido_materno = pe.apellido_materno,
                                roleId = ro?.Id ?? 6, // 6 id usuario normal
                                role = ro?.role ?? "usuario"
                            }).ToList();
            return response;
        }
        public async Task<userRole> updateUserRole(DTOUserRole request)
        {
            var role = await _db.userRoles.FirstOrDefaultAsync(x => x.personaId == request.personaId);
            if(role == null)
            {
                var newRole = new userRole
                {
                  roleId=request.roleId,
                  personaId = request.personaId,
                  userId = request.userId  
                };
                _db.userRoles.Add(newRole);
                await _db.SaveChangesAsync();
                return newRole;
            }
            role.roleId = request.roleId;
            await _db.SaveChangesAsync();
            return role;
        }
    }
}