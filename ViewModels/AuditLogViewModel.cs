using System;
using System.Collections.Generic;

namespace ClaimsMVC.ViewModels
{
    public class AuditLogViewModel
    {
        public DateTime Timestamp { get; set; }
        public required string UserName { get; set; }
       

        
        public required string Summary { get; set; }
    }
}
