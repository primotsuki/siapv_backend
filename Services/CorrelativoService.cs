using Microsoft.EntityFrameworkCore;
using siapv_backend.DB;
using siapv_backend.Models;

namespace siapv_backend.Services
{
    public class CorrelativoService: ICorrelativoService
    {
        public readonly AppDbContext db;
        public CorrelativoService(AppDbContext _db)
        {
            db = _db;
        }
        public async Task<int> getCorrelativoProceso(int procesoId)
        {
            int year = DateTime.UtcNow.Year;
            var correlativo = await db.correlativos.FirstOrDefaultAsync(x=>x.procesointernoId==procesoId && x.gestion==year);
            if (correlativo == null)
            {
                Correlativo corr = new Correlativo
                {
                    correlativo = 1,
                    gestion = year,
                    procesointernoId = procesoId
                };
                db.correlativos.Add(corr);
                await db.SaveChangesAsync();
                return 1;
            }
            var correla = correlativo.correlativo +1;
            correlativo.correlativo = correla;
            await db.SaveChangesAsync();
            return correla;
        }
    }
}