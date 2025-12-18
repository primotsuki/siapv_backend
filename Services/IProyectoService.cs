using siapv_backend.Models;

namespace siapv_backend.Services
{
    public interface IProyectoService
    {
        Task<Proyecto?> createProyectos(string nombre);
        Task<List<Proyecto>> getProyectosList();
    }
}