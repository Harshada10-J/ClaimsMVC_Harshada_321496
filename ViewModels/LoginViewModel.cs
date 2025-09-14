
using System.ComponentModel.DataAnnotations;

namespace ClaimsMVC.ViewModels
{
    public class LoginViewModel
    {
        [Required]
   
        public string EmployeeNo { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}