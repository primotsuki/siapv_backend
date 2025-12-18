using siapv_backend.DB;
using siapv_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace siapv_backend.Services
{
    public class FinanciamientoService: IFinanciamientoService
    {
        public readonly AppDbContext db;
        public FinanciamientoService(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<List<FuenteFinanciamiento>> getFinanciamientoList()
        {
            var query = await (from f in db.fuenteFinanciamientos
                         select f).ToListAsync();
            return query;
        }
    }
}