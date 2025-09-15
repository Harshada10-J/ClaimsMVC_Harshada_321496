using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using ClaimsMVC.Data.Models;
using ClaimsMVC.ViewModels;
using ClaimsMVC.Services;
using Microsoft.EntityFrameworkCore;

namespace ClaimsMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;
        private readonly ClaimsContextDB _context;
        private readonly AuditService _auditService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, EmailService emailService, AuditService auditService, ClaimsContextDB context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _auditService = auditService;

            _context = context;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
         
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.EmployeeNo);
                if (user != null  && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    
                    var previousLoginTime = user.LastLoginTime;

                   
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        user.LastLoginTime = DateTime.UtcNow;
                        await _userManager.UpdateAsync(user);

                        
                        TempData["PreviousLoginTime"] = previousLoginTime?.ToLocalTime().ToString("f");
                        await _auditService.LogActionAsync("User Logged In", $"User '{user.FullName}' ({user.EmployeeNo}) logged in.");
                        return LocalRedirect(returnUrl ?? "/");
                    }
                }
                
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {

                
                var companyEmployee = await _context.CompanyEmployees
                    .FirstOrDefaultAsync(e => e.EmployeeNo == model.EmployeeNo);

                if (companyEmployee == null)
                {
                    ModelState.AddModelError("EmployeeNo", "This employee number is not registered with the company.");
                    return View(model);
                }

                
                var existingUser = await _userManager.FindByNameAsync(model.EmployeeNo);
                if (existingUser != null)
                {
                    ModelState.AddModelError("EmployeeNo", "An online account has already been registered for this employee number.");
                    return View(model);
                }

          
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = model.EmployeeNo,
                    EmployeeNo = model.EmployeeNo,
                    FullName = model.FullName,
                    Email = model.Email,
                   
                    DesignationId = companyEmployee.DesignationId,
                    EmailConfirmed = true 
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Employee");

                   
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    await _auditService.LogActionAsync("User Registered", $"New user '{user.FullName}' ({user.EmployeeNo}) registered an account.");

                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }



        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                // --- LOG THE LOGOUT ACTION ---
                await _auditService.LogActionAsync("User Logged Out", $"User '{user.FullName}' ({user.EmployeeNo}) logged out.");
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}

