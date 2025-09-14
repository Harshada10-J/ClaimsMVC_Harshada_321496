using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.ViewModels
{
    public class SubmitClaimViewModel
    {
        public List<ClaimItemViewModel> Items { get; set; } = new List<ClaimItemViewModel> { new ClaimItemViewModel() };
    }

    public class ClaimItemViewModel
    {
        [Required]
        [Display(Name = "Item Name")]
        public string ItemName { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cost must be a positive number.")]
        public decimal Cost { get; set; }
    }
}
