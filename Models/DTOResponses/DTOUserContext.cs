namespace siapv_backend.Models.DTOResponses
{
    public class DTOUserContext
    {
        public int userId {  get; set; }
        public int personaId { get; set; }
        public int? roleId { get; set; }
        public string username { get; set; } = null!;
        public char? sexo { get; set; }

    }
}
