using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.ViewModels
{
    /// <summary>
    /// This model is for the form where a CPD user can edit an employee's details.
    /// </summary>
    public class EditEmployeeViewModel
    {
        public string Id { get; set; }

        [Display(Name = "Employee Number")]
        public string EmployeeNo { get; set; } // Usually read-only

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Designation")]
        public int? DesignationId { get; set; }

        // This list will be used to populate the dropdown menu for designations.
        public List<SelectListItem> AvailableDesignations { get; set; } = new List<SelectListItem>();
    }
}
