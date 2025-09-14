using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsMVC.Data.Models
{
    public class Claim
    {
        [Key]
        public int ClaimId { get; set; }
        public required string EmployeeId { get; set; }
        public DateTime SubmissionDate { get; set; }
        public required string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? ApprovedAmount { get; set; }

        public string? RejectionReason { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual User? User { get; set; }
        public virtual ICollection<ClaimItem> ClaimItems { get; set; } = new List<ClaimItem>();
    }
}
