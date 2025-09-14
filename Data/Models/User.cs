
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsMVC.Data.Models; 

public class User : IdentityUser<string>
{
    public required string EmployeeNo { get; set; }
    public required string FullName { get; set; }

    public int? DesignationId { get; set; }
    [ForeignKey("DesignationId")]
    public virtual Designation? Designation { get; set; }
    public DateTime? LastLoginTime { get; set; }
}

