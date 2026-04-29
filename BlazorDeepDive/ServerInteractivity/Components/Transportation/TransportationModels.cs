namespace ServerInteractivity.Components.Transportation;

public enum TransportationStatus
{
    Saved,
    Submitted,
    Returned,
    Approved
}

public sealed class TransportationEvent
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public TransportationStatus Status { get; set; }
}

public sealed class MessageContact
{
    public int Id { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty; // Email / Phone / SMS
    public string Message { get; set; } = string.Empty;
    public DateTime SentOn { get; set; }
}

public sealed class StatusHistoryEntry
{
    public TransportationStatus Status { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
    public DateTime ChangedOn { get; set; }
    public string Notes { get; set; } = string.Empty;
}

public sealed class ContactInfo
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public sealed class Announcement
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Today;
}

public enum UserRole
{
    Admin,
    Viewer
}
