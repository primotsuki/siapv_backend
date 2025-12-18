using Microsoft.EntityFrameworkCore;
using siapv_backend.Models;

namespace siapv_backend.DB
{
    public class DBUsuariosContext: DbContext
    {
        public DBUsuariosContext(DbContextOptions<DBUsuariosContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Dependencia> Dependencias { get; set; }
        public DbSet<EmpleadosContrato> EmpleadosContratos { get; set; }
    }   
}