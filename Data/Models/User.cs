
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsMVC.Data.Models; // Or your actual project namespace

public class User : IdentityUser<string>
{
    // All standard properties like Id, UserName, Email, PasswordHash, etc.,
    // are inherited automatically from the IdentityUser base class.

    // ONLY your custom properties should be listed here.
    public required string EmployeeNo { get; set; }
    public required string FullName { get; set; }

    public int? DesignationId { get; set; }
    [ForeignKey("DesignationId")]
    public virtual Designation? Designation { get; set; }
   
}

