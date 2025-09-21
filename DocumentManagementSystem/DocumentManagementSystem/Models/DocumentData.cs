using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace DocumentManagementSystem.Models
{
    public class DocumentData
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string Summary { get; set; }
        public string Original { get; set; }
        public string Tags { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
