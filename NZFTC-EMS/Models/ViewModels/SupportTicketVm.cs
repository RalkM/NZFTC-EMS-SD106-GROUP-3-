namespace NZFTC_EMS.Models.ViewModels;

public record SupportTicketRowVm(
    int Id,
    string Subject,
    string Preview,
    string Status,
    string Priority,
    DateTime CreatedAt,
    string? EmployeeName,
    string? EmployeeCode,
    int? EmployeeId
);

public class SupportTicketCreateVm
{
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium";
}
