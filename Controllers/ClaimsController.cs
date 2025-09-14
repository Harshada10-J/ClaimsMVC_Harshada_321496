

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ClaimsMVC.Data.Models;
using ClaimsMVC.ViewModels;
using ClaimsMVC.Services;
using SecurityClaim = System.Security.Claims.Claim;

namespace ClaimsMVC.Controllers
{
    [Authorize]
    public class ClaimsController : Controller
    {
        private readonly ClaimsContextDB _context;
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;
        private readonly IConfiguration _configuration;

        public ClaimsController(ClaimsContextDB context, UserManager<User> userManager, EmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
        }

        // Action for submitting a new claim
        [HttpGet]
        [Authorize(Roles = "Employee")]
        public IActionResult Submit()
        {
            return View(new SubmitClaimViewModel());
        }
        [HttpPost]
        [Authorize(Roles = "Employee")]
        public async Task<IActionResult> Submit(SubmitClaimViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var newClaim = new ClaimsMVC.Data.Models.Claim
                {
                    EmployeeId = userId,
                    SubmissionDate = DateTime.UtcNow,
                    Status = "Pending",
                    TotalAmount = model.Items.Sum(item => item.Cost),
                    ClaimItems = model.Items.Select(item => new ClaimItem
                    {
                        ItemName = item.ItemName,
                        Cost = item.Cost
                    }).ToList()
                };

                _context.Claims.Add(newClaim);
                await _context.SaveChangesAsync();

                return RedirectToAction("History"); 
            }
            return View(model);
        }
     
        [HttpGet]
        public async Task<IActionResult> History(string? status, int? claimId, DateTime? submissionDate)
        {
            
            IQueryable<ClaimsMVC.Data.Models.Claim> claimsQuery = _context.Claims.Include(c => c.User);

            if (User.IsInRole("CPD"))
            {
                if (!string.IsNullOrEmpty(status))
                {
                    claimsQuery = claimsQuery.Where(c => c.Status == status);
                }
                if (claimId.HasValue)
                {
                    claimsQuery = claimsQuery.Where(c => c.ClaimId == claimId.Value);
                }
                if (submissionDate.HasValue)
                {
                    claimsQuery = claimsQuery.Where(c => c.SubmissionDate.Date == submissionDate.Value.Date);
                }
            }
           
            else if (User.IsInRole("Employee"))
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    claimsQuery = claimsQuery.Where(c => c.EmployeeId == userId);
                }
            }

            var claims = await claimsQuery.OrderByDescending(c => c.SubmissionDate).ToListAsync();

            ViewData["CurrentStatus"] = status;
            ViewData["CurrentClaimId"] = claimId;
            ViewData["CurrentDate"] = submissionDate?.ToString("yyyy-MM-dd");

            return View(claims);
        }

        // Action for viewing the details of a single claim
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var claim = await _context.Claims.Include(c => c.User).Include(c => c.ClaimItems).FirstOrDefaultAsync(c => c.ClaimId == id);
            if (claim == null || claim.User == null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.IsInRole("Employee") && !User.IsInRole("CPD") && claim.EmployeeId != currentUserId)
            {
                return Forbid();
            }

            var model = new ClaimDetailsViewModel
            {
                ClaimId = claim.ClaimId,
                SubmissionDate = claim.SubmissionDate,
                Status = claim.Status,
                TotalAmount = claim.TotalAmount,
                ApprovedAmount = claim.ApprovedAmount,
                RejectionReason = claim.RejectionReason,
                EmployeeNo = claim.User.EmployeeNo,
                FullName = claim.User.FullName,
                Items = claim.ClaimItems.Select(ci => new ClaimItemDetail
                {
                    ItemName = ci.ItemName,
                    Cost = ci.Cost
                }).ToList()
            };
            return View(model);
        }

        // Actions for approving a claim
        [HttpGet]
        [Authorize(Roles = "CPD")]
        public async Task<IActionResult> Approve(int id)
        {
            var claim = await _context.Claims.Include(c => c.User).ThenInclude(u => u.Designation).FirstOrDefaultAsync(c => c.ClaimId == id);
            if (claim == null) return NotFound();
            var totalApprovedSoFar = await _context.Claims
               .Where(c => c.EmployeeId == claim.User.Id && c.Status == "Approved")
               .SumAsync(c => c.ApprovedAmount ?? 0);

            var claimLimit = claim.User.Designation?.ClaimLimit ?? 0;
            var remainingAmount = claimLimit - totalApprovedSoFar;
            var viewModel = new ApproveClaimViewModel
            {
                ClaimId = claim.ClaimId,
                TotalAmount = claim.TotalAmount,
                EmployeeName = claim.User.FullName,

                EmployeeClaimLimit = claim.User.Designation?.ClaimLimit ?? 0,
                RemainingClaimableAmount = remainingAmount,
            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CPD")]
        public async Task<IActionResult> Approve(ApproveClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var claim = await _context.Claims.Include(c => c.User).ThenInclude(u => u.Designation).FirstOrDefaultAsync(c => c.ClaimId == model.ClaimId);
                if (claim == null) return NotFound();

                if (claim == null || claim.User == null)
                {
                    return NotFound();
                }
                if (claim.User.Designation == null)
                {
                    ModelState.AddModelError("", "This employee does not have a designation and claim limit assigned.");
                    return View(model);
                }
            
                if (model.ApprovedAmount > claim.TotalAmount)
                {
                    ModelState.AddModelError("ApprovedAmount", $"Approved amount cannot exceed the total claimed amount of {claim.TotalAmount}.");
                    return View(model);
                }
                var totalApprovedSoFar = await _context.Claims
                         .Where(c => c.EmployeeId == claim.User.Id && c.Status == "Approved")
                         .SumAsync(c => c.ApprovedAmount ?? 0);
                var remainingAmount = claim.User.Designation.ClaimLimit - totalApprovedSoFar;

                // 2. NEW: Check if the approved amount exceeds the remaining balance.
                if (model.ApprovedAmount > remainingAmount)
                {
                    ModelState.AddModelError("ApprovedAmount", $"Approved amount exceeds the employee's remaining balance of {remainingAmount:C}.");
                    return View(model);
                }

                if (model.ApprovedAmount > claim.User.Designation.ClaimLimit)
                {
                    ModelState.AddModelError("ApprovedAmount", $"Approved amount exceeds the employee's designation claim limit of {claim.User.Designation.ClaimLimit}.");
                    return View(model);
                }

                claim.Status = "Approved";
                claim.ApprovedAmount = model.ApprovedAmount;
                await _context.SaveChangesAsync();

                // Send notifications
                var bankEmail = _configuration["BankEmailAddress"];
                var employeeEmail = claim.User.Email;
                if (!string.IsNullOrEmpty(bankEmail))
                {
                    var bankSubject = $"Action Required: Reimbursement for {claim.User.FullName}";
                    var bankContent = $"<p>Please credit the account of employee {claim.User.FullName} (Employee No: {claim.User.EmployeeNo}) for the amount of ₹{claim.ApprovedAmount}.</p><p>Reference Claim ID: {claim.ClaimId}</p>";
                    await _emailService.SendEmailAsync(bankEmail, bankSubject, bankContent);
                }


                if (!string.IsNullOrEmpty(employeeEmail))
                {
                    var employeeSubject = $"Your Claim (ID: {claim.ClaimId}) Has Been Approved";
                    var employeeContent = $"<p>Hello {claim.User.FullName},</p><p>Your claim with ID {claim.ClaimId} has been approved for the amount of ₹{claim.ApprovedAmount}.</p><p>The amount will be credited to your account shortly.</p>";
                    await _emailService.SendEmailAsync(employeeEmail, employeeSubject, employeeContent);
                }
                return RedirectToAction("History");
            }
            return View(model);
        }
        // Actions for rejecting a claim

        [HttpGet]
        [Authorize(Roles = "CPD")]
        public async Task<IActionResult> Reject(int id)
        {
            var claim = await _context.Claims.Include(c => c.User).FirstOrDefaultAsync(c => c.ClaimId == id);
            if (claim == null || claim.User == null)
            {
                return NotFound();
            }


            var viewModel = new RejectClaimViewModel
            {
                ClaimId = claim.ClaimId,
                EmployeeName = claim.User.FullName
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "CPD")]
        public async Task<IActionResult> Reject(RejectClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {


                var claim = await _context.Claims.FindAsync(model.ClaimId);
                if (claim == null) return NotFound();

                claim.Status = "Rejected";
                claim.RejectionReason = model.Reason;
                claim.ApprovedAmount = 0;
                await _context.SaveChangesAsync();

                return RedirectToAction("History");
            }
            return View(model);
        }
    }
}


