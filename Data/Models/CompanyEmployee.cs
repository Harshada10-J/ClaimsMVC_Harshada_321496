using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsMVC.Data.Models
{
    public class CompanyEmployee
    {
        [Key]
        public int Id { get; set; }
        public required string EmployeeNo { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public int DesignationId { get; set; }

        [ForeignKey("DesignationId")]
        public virtual Designation? Designation { get; set; }
    }
}