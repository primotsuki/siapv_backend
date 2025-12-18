using siapv_backend.DB;
using siapv_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace siapv_backend.Services
{
    public class ProyectoService: IProyectoService
    {
        public readonly AppDbContext db;
        public ProyectoService(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<Proyecto?> createProyectos(string proyecto)
        {
            var proy = new Proyecto
            {
                descripcion = proyecto
            };
            db.proyectos.Add(proy);
            var result = await db.SaveChangesAsync();
            return result >= 0 ? proy : null;
        }
        public async Task<List<Proyecto>>getProyectosList()
        {
            var query = await (from p in db.proyectos
                         select p).ToListAsync();
            return query;
        }
    }
}