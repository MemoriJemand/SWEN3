namespace DocumentManagementSystem.Models
{
    public class DocIndex
    {
        public string DocID { get; set; } = default!;
        public string Content { get; set; } = default!;
        public DateTime Timestamp { get; set; }
    }
}
