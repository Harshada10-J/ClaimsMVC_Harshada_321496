using System.Collections.Generic;
using ClaimsMVC.Data.Models;

namespace ClaimsMVC.ViewModels
{
    public class CpdDashboardViewModel
    {
        public int PendingClaimsCount { get; set; }
        public List<Claim> RecentPendingClaims { get; set; } = new List<Claim>();
    }
}
