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

             public int EmployeeId { get; set; }
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
            public DateTime? NextPayrollRunDate { get; set; }
    public DateTime? LastPayrollPeriodStart { get; set; }
    public DateTime? LastPayrollPeriodEnd { get; set; }

        // PAYROLL OVERVIEW
        public string NextPayrollRunLabel { get; set; } = "Not scheduled";
        public string LatestTotalsLabel { get; set; } = "No payroll runs yet";

         public List<RecentActivityItemVm> RecentActivity { get; set; } = new();
        public List<UpcomingEventVm>     UpcomingEvents  { get; set; } = new();
        public List<AnnouncementVm>      Announcements   { get; set; } = new();
    }

    public class RecentActivityItemVm
    {
        public DateTime When { get; set; }
        public string Message { get; set; } = "";
        public string Category { get; set; } = ""; // "leave", "ticket", "payroll"
    }

    public class UpcomingEventVm
    {
        public DateTime Date { get; set; }
        public string Label { get; set; } = "";
        public string Category { get; set; } = ""; // "leave", "payroll", "birthday"
    }

    public class AnnouncementVm
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }
}