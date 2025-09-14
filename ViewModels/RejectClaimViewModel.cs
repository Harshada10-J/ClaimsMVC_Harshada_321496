using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.ViewModels
{
    public class RejectClaimViewModel
    {
        public int ClaimId { get; set; }
        public string EmployeeName { get; set; }

        [Required]
        [Display(Name = "Reason for Rejection")]
        public string Reason { get; set; }
    }
}
