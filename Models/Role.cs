using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
namespace siapv_backend.Models
{
    public class Role
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(40)")]
        public string role { get; set; } = null!;
    }
}