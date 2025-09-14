using System;
using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.Data.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }
        public string? UserId { get; set; }
        public required string Action { get; set; } // e.g., "Added", "Modified", "Deleted"
        public required string EntityType { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Changes { get; set; } // Will store a JSON string of changes
    }
}