using System.Security.Claims;
using System.Threading.Tasks;
using ClaimsMVC.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ClaimsMVC.Services
{
    public class AuditService
    {
        private readonly ClaimsContextDB _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuditService(ClaimsContextDB context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActionAsync(string action, string description)
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var auditLog = new AuditLog
            {
                UserId = userId,
                Timestamp = DateTime.UtcNow,
                Action = action,
                Description = description
            };

            _context.AuditLogs.Add(auditLog);
            await _context.SaveChangesAsync();
        }
    }
}