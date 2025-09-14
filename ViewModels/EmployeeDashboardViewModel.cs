using System.Collections.Generic;
using ClaimsMVC.Data.Models;

namespace ClaimsMVC.ViewModels
{
    public class EmployeeDashboardViewModel
    {
        public required string FullName { get; set; }
        public List<Claim> RecentClaims { get; set; } = new List<Claim>();
    }
}
