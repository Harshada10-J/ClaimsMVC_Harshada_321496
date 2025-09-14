using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.ViewModels
{
    public class ApproveClaimViewModel
    {
        public int ClaimId { get; set; }

        [Display(Name = "Employee Name")]
        public required string EmployeeName { get; set; }

        [Display(Name = "Total Amount Claimed")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal TotalAmount { get; set; }
        [Display(Name = "Employee's Claim Limit")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal EmployeeClaimLimit { get; set; }
        [Display(Name = "Remaining Balance")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal RemainingClaimableAmount { get; set; }
        [Required]
        [Display(Name = "Amount to Approve")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Approved amount must be greater than zero.")]
        public decimal ApprovedAmount { get; set; }
    }
}

