namespace siapv_backend.Services
{
    public interface ICorrelativoService
    {
        Task<int> getCorrelativoProceso(int procesoId);
    }
}