using System.ComponentModel.DataAnnotations;
using NZFTC_EMS.Models.ViewModels;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Models.ViewModels
{
    public class EmployeeDashboardVm
    {
        public decimal AnnualSalary { get; set; }
        public decimal YtdEarnings { get; set; }
        public double AnnualLeaveHours { get; set; }
        public double SickLeaveHours { get; set; }

        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public string Gender { get; set; }
    }

public class AdminDashboardVm
    {
        public int TotalEmployees { get; set; }
        public int ActiveLeave { get; set; }
        public int PendingLeave { get; set; }
        public int OpenGrievances { get; set; }

        // optional extras used in the UI
        public int PendingLeaveRequests { get; set; }
        public int PendingSupportTickets { get; set; }
    }
}