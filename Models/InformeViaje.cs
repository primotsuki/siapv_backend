using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend.Models
{
    public class InformeViaje
    {
        public int Id {get; set;}
        public int solicitudId {get; set;}
        [Column(TypeName = "varchar(100)")]
        public string cite_doc {get; set;} = null!;
        public SolicitudViaje? solicitud {get; set;}
        public string antecedentes {get; set;} = null!;
        public string desarrollo {get; set;} = null!;
        public string conclusion {get; set;} = null!;
        public DateTime createdAt {get; set;}
        public DateTime updatedAt {get; set;}
    }
}