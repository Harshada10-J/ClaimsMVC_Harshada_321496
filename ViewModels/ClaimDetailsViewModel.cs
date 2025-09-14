using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.ViewModels
{
    public class ClaimDetailsViewModel
    {
        public int ClaimId { get; set; }

        [Display(Name = "Submission Date")]
        public DateTime SubmissionDate { get; set; }

        public required string Status { get; set; }

        [Display(Name = "Total Amount Claimed")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Amount Approved")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal? ApprovedAmount { get; set; }

        [Display(Name = "Reason for Rejection")]
        public string? RejectionReason { get; set; }

        [Display(Name = "Employee No.")]
        public required string EmployeeNo { get; set; }

        [Display(Name = "Full Name")]
        public required string FullName { get; set; }

        public List<ClaimItemDetail> Items { get; set; } = new List<ClaimItemDetail>();
    }

    public class ClaimItemDetail
    {
        public required string ItemName { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Cost { get; set; }
    }
}

