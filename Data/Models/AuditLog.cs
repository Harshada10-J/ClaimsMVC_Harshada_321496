using System;
using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.Data.Models
{
    public class AuditLog
    {
        [Key]
        public int AuditLogId { get; set; }
        public string? UserId { get; set; }
        public DateTime Timestamp { get; set; }
        public required string Action { get; set; }
        public string? Description { get; set; }
    }
}