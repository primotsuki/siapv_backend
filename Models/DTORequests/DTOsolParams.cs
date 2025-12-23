namespace siapv_backend.Models.DTORequests
{
    public class DTOsolParams
    {
        public string? searchTerm {get; set;} = null!;
        public int pageSize {get; set; }
        public int page {get; set;}
        public int estado {get; set;}
    }
}