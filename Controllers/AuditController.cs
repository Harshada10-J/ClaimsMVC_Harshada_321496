using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using ClaimsMVC.Data.Models;

namespace ClaimsMVC.Controllers
{
    [Authorize(Roles = "CPD")]
    public class AuditController : Controller
    {
        private readonly ClaimsContextDB _context;
        public AuditController(ClaimsContextDB context) 
        { 
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var logs = await _context.AuditLogs
                .OrderByDescending(a => a.Timestamp)
                .Take(100)
                .ToListAsync();
            return View(logs);
        }
    }
}