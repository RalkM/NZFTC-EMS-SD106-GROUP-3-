namespace NZFTC_EMS.ViewModels;

public record SupportTicketRowVm(
    int Id,
    string Subject,
    string Preview,
    string Status,
    string Priority,
    DateTime CreatedAt
);

public class SupportTicketCreateVm
{
    public string Subject { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Priority { get; set; } = "Medium"; // Low/Medium/High/Urgent
}

