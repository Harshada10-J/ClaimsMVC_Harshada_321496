using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ClaimsMVC.Data.Models;
using ClaimsMVC.ViewModels;

namespace ClaimsMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ClaimsContextDB _context;

        public HomeController(UserManager<User> userManager, ClaimsContextDB context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Check if the user is authenticated
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    // This should not happen for a logged-in user, but it's a safe fallback.
                    return Challenge();
                }

                // If the user is in the "CPD" role, show them the CPD dashboard
                if (await _userManager.IsInRoleAsync(user, "CPD"))
                {
                    var pendingClaims = _context.Claims.Where(c => c.Status == "Pending");
                    var viewModel = new CpdDashboardViewModel
                    {
                        PendingClaimsCount = await pendingClaims.CountAsync(),
                        RecentPendingClaims = await pendingClaims.OrderByDescending(c => c.SubmissionDate).Take(5).ToListAsync()
                    };
                    return View("CpdDashboard", viewModel);
                }

                // If the user is in the "Employee" role, show them their dashboard
                if (await _userManager.IsInRoleAsync(user, "Employee"))
                {
                    var viewModel = new EmployeeDashboardViewModel
                    {
                        FullName = user.FullName,
                        RecentClaims = await _context.Claims
                           .Where(c => c.EmployeeId == user.Id)
                           .OrderByDescending(c => c.SubmissionDate)
                           .Take(5)
                           .ToListAsync()
                    };
                    return View("EmployeeDashboard", viewModel);
                }
            }

            // If the user is not logged in, show the default public home page
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

