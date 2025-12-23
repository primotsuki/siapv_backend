using System.Security.Cryptography.Pkcs;

namespace siapv_backend.Models
{
    public class InformeFinal
    {
        public int Id {get; set;}
        public int solicitudId {get; set;}
        public SolicitudViaje? solicitud {get; set;}
        public string antecedentes {get; set;} = null!;
        public string desarrollo {get; set;} = null!;
        public string conclusion {get; set;} = null!;
        public DateTime createdAt {get; set;}
        public DateTime updatedAt {get; set;}
    }
}