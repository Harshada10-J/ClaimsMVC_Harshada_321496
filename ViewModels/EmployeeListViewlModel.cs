using System.Collections.Generic;

namespace ClaimsMVC.ViewModels
{
    /// <summary>
    /// This model will be used to display a list of all employees to the CPD.
    /// </summary>
    public class EmployeeListViewModel
    {
        public List<EmployeeSummaryItem> Employees { get; set; } = new List<EmployeeSummaryItem>();
    }

    public class EmployeeSummaryItem
    {
        public string Id { get; set; }
        public string EmployeeNo { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string DesignationName { get; set; }
    }
}
