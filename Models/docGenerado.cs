using System.ComponentModel.DataAnnotations.Schema;

namespace siapv_backend
{
    public class DocGenerado
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string cite_documento { get; set; } = null!;
        public int id_documento { get; set; }
        public DateTime createdAt {get; set; }
    }
}