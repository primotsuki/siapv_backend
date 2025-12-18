namespace siapv_backend.Models.DTORequests
{
    public class DTOUserParams
    {
        public required int pageSize { get; set; }
        public required int page { get; set; }
        public string? searchTerm { get; set; } = null!;
    }
}
