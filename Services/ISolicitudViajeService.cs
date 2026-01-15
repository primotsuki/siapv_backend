using siapv_backend.Models;
using siapv_backend.Models.DTORequests;
using siapv_backend.Models.DTOResponses;
namespace siapv_backend.Services
{
    public interface ISolicitudViajeService
    {
        Task<List<LugarDestino>> GetDestinos();
        Task<SolicitudViaje?> generarMemorandum(DTOMemorandum memo,int designadorId);
        Task<List<DTOSolicitudViaje>> getSolicitudes(int empleadoId);
        Task<List<DTOSolicitudesPendientes>> getPendientes(int empleadoId);
        Task<SolicitudViaje?> completarSolicitud(DTOFucav solicitudId);
        Task<SolicitudViaje?> getSolicitudById(int solicitudId);
        Task<List<MediosTransporte>> getMediosTransporte();
        Task<List<DTOEmpleadoSolicitud>> getSolicitudesByDependencia(int dependenciaId);
        Task<List<TipoViaje>> getTiposViaje();
        Task<List<DTOSolicitudEstado>> getSolicitudByEstados(DTOsolicitudReq request);
        Task<List<DTOSolicitudesPasajes>> getSolicitudesbyParams(DTOsolParams request);
        Task<RevisionFormularios?> crearRevisiondeFormulario(DTORevision request);
        Task<List<EstadoSolicitud>> getEstados();
        Task<InformeViaje> createInforme(DTOInformeViaje request);
        Task<Byte[]> getInformeViaje(int solicitudId);
        Task<InformeViaje> getInformeEdit(int solicitudId);
        Task<InformeViaje> editInforme(DTOInformeViaje request);
        
    }
}