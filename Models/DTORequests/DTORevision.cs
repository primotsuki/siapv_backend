namespace siapv_backend.Models
{
    public class DTORevision
    {
        public int solicitudId {get; set;} 
        public bool fucav { get; set;}
        public bool poa {get; set;}
        public bool presupuesto {get; set;}
        public bool memo {get; set;}
        public bool informe {get; set;}
    }
}