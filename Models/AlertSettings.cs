namespace SecureChild.Models
{
    public class AlertSettings
    {
        public int Id { get; set; }
        public bool SmsEnabled { get; set; } // Enable/Disable SMS notifications
        public bool EmailEnabled { get; set; } // Enable/Disable Email notifications
        public bool EntryNotificationEnabled { get; set; } // Enable/Disable Entry notifications
        public bool ExitNotificationEnabled { get; set; } // Enable/Disable Exit notifications
    }
}