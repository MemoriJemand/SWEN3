using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel.DataAnnotations;

namespace DocumentManagementSystem.Models
{
    public class DocumentData
    {
        [Key]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public string Original { get; set; } = string.Empty;
        public string Tags { get; set; } = string.Empty;
        public DateTime DateUploaded { get; set; }
    }
}
