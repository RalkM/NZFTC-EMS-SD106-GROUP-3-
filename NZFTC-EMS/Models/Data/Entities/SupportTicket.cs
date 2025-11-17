using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NZFTC_EMS.Data.Entities
{
    // Enums FIRST, inside the SAME namespace
    public enum SupportStatus   { Open = 0, InProgress = 1, Resolved = 2, Closed = 3 }
    public enum SupportPriority { Low  = 0, Medium     = 1, High     = 2, Urgent  = 3 }

    public class SupportTicket
    {
        [Key] public int Id { get; set; }

        [Required, MaxLength(120)]
        public string Subject { get; set; } = string.Empty;

        [Required, MaxLength(4000)]
        public string Message { get; set; } = string.Empty;

        [Required] public SupportStatus Status { get; set; } = SupportStatus.Open;
        [Required] public SupportPriority Priority { get; set; } = SupportPriority.Medium;

        public int? EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        public int? AssignedToId { get; set; }

        [Required] public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public List<SupportMessage> Messages { get; set; } = new List<SupportMessage>();

        public SupportTicket()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }

    public class SupportMessage
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   // ✅ ONLY HERE
    public int Id { get; set; }

    public int TicketId { get; set; }
    public SupportTicket Ticket { get; set; } = null!;

    public int? SenderEmployeeId { get; set; }

    [Required, MaxLength(4000)]
    public string Body { get; set; } = string.Empty;

    [Required]
    public DateTime SentAt { get; set; }

    public bool SenderIsAdmin { get; set; }

    public string? AdminReply { get; set; }
    public DateTime? AdminReplyAt { get; set; }

    
}

    
    
}
