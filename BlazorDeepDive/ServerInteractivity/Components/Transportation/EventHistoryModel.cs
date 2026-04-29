// ============================================================
// FILE: EventHistoryModel.cs
// PURPOSE: Model class used by EventHistory and TransportEvent.
//          Every property maps to one column in the history table.
// ============================================================

namespace ServerInteractivity.Components.Transportation
{
    public class EventHistoryModel
    {
        // ── Table Columns ──────────────────────────────────────

        /// <summary>Date this record was last updated (e.g. "04/28/2025")</summary>
        public string UpdateDate { get; set; } = string.Empty;

        /// <summary>Current status (e.g. "New", "Approved", "Rejected")</summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>Who made the update</summary>
        public string UpdatedBy { get; set; } = string.Empty;

        /// <summary>The date of the actual event</summary>
        public string EventsDate { get; set; } = string.Empty;

        /// <summary>School year this event belongs to (e.g. "2024-2025")</summary>
        public string SchoolYear { get; set; } = string.Empty;

        /// <summary>Stage/label of the status (e.g. "Event Start")</summary>
        public string StatusDate { get; set; } = string.Empty;

        /// <summary>Comment related to certification</summary>
        public string CertComment { get; set; } = string.Empty;

        /// <summary>Certifications value (e.g. "Yes" / "No")</summary>
        public string Certifications { get; set; } = string.Empty;

        /// <summary>Comments from the manager level</summary>
        public string IfManagerComments { get; set; } = string.Empty;

        /// <summary>Final admin-level comments</summary>
        public string AdminComments { get; set; } = string.Empty;
    }
}
