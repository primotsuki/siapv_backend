using Microsoft.EntityFrameworkCore;
using siapv_backend.Models;
namespace siapv_backend.DB
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<FuenteFinanciamiento> fuenteFinanciamientos { get; set; }  
        public DbSet<MediosTransporte> mediosTransportes { get; set; }
        public DbSet<Proyecto> proyectos { get; set; }
        public DbSet<TipoViaje> tiposViaje { get; set; }
        public DbSet<LugarDestino> lugarDestinos{ get; set; }
        public DbSet<DocGenerado> docGenerados{ get; set; }
        public DbSet<SolicitudViaje> solicitudViajes{ get; set; }
        public DbSet<EstadoSolicitud> estadoSolicitudes{ get; set; }
        public DbSet<CategoriaProgramatica> categoriasProgramaticas { get; set; }
        public DbSet<Correlativo> correlativos {get; set;}
        public DbSet<CertificacionPresupuestaria> certificacionPresupuestarias {get; set;}
        public DbSet<ActividadPOA> actividadPOAs {get; set;}
        public DbSet<OperacionPOA> operacionPOAs { get; set; }
        public DbSet<CertificacionPOA> certificacionPOAs {get; set;}
        public DbSet<Role> roles {get; set; }
        public DbSet<userRole> userRoles {get; set;}
        public DbSet<Reprogramacion> reprogramaciones {get; set;}
        public DbSet<RevisionFormularios> revisiones {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EstadoSolicitud>().HasData(
                new EstadoSolicitud { Id = 1, estado = "Designado" },
                new EstadoSolicitud { Id = 2, estado = "Solitado" },
                new EstadoSolicitud { Id = 3, estado = "Pendiente" },
                new EstadoSolicitud { Id = 4, estado = "Enviado a revisión" },
                new EstadoSolicitud { Id = 5, estado = "Aprobado" },
                new EstadoSolicitud { Id = 6, estado = "Finalizado" }    
            );
        }
    }
}