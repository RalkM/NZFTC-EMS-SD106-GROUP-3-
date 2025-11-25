using System.ComponentModel.DataAnnotations;
using NZFTC_EMS.Models.ViewModels;
using NZFTC_EMS.Data.Entities;

namespace NZFTC_EMS.Models.ViewModels
{
    public class EmployeeDashboardVm
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; } = "";

        // PAY / EARNINGS
        public decimal AnnualSalary { get; set; }
        public decimal YtdEarnings { get; set; }

        // LEAVE BALANCES
        public decimal AnnualLeaveHours { get; set; }
        public decimal SickLeaveHours { get; set; }

        // PROFILE
        public DateTime Birthday { get; set; } = DateTime.MinValue;
        public string? Gender { get; set; }
        public List<LeaveRequest> UpcomingLeave { get; set; } = new();
public List<LeaveRequest> PastLeave     { get; set; } = new();

    }
    

 public class AdminDashboardVm
    {
        // EMPLOYEE SUMMARY
        public int TotalEmployees { get; set; }
        public int ActiveLeave { get; set; }
        public int PendingLeave { get; set; }
        public int PendingLeaveRequests { get; set; }

        // SUPPORT / GRIEVANCES
        public int PendingSupportTickets { get; set; }
        public int OpenGrievances { get; set; }

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