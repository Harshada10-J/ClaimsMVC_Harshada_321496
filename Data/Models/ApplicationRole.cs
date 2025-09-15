

using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClaimsMVC.Data.Models;

public class ApplicationRole : IdentityRole
{
    public ApplicationRole() : base() { }

    public ApplicationRole(string roleName) : base(roleName) { }
}
