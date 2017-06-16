namespace Tandeo2.Models
{
    public class TandaItem
    {
        public string id { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string version { get; set; }
        public bool deleted { get; set; }
        public string nombre { get; set; }
        public decimal deuda { get; set; }
    }
}