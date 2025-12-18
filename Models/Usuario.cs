using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Xml.Linq;


namespace siapv_backend.Models
{
    [Index(nameof(username), Name = "Index_UserName")]
    public class Usuario
    {
        public int Id {  get; set; }
        [Column(TypeName= "varchar(40)")]
        public required string username { get; set; }
        [Column(TypeName ="varchar(60)")]
        public required string email { get; set; }
        public DateTime created_at { get; set; }
        public int PersonaId { get; set; }
        public Persona Persona { get; set; } = null!;
        [JsonIgnore]
        public DateTime? last_login { get; set; }
        [DefaultValue(true)]
        public required Boolean? isActive { get; set; }
    }
}