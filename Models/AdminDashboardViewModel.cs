namespace SecureChild.Models
{
    public class AdminDashboardViewModel
    {
        public int StudentsCount { get; set; }
        public int SecurityStaffCount { get; set; }
        public IEnumerable<Entry> RecentEntries { get; set; }
        public IEnumerable<Exit> RecentExits { get; set; }
        public IEnumerable<Alert> Alerts { get; set; }
    }
}