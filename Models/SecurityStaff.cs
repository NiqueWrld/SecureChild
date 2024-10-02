using System.ComponentModel.DataAnnotations;

namespace SecureChild.Models
{
    public class SecurityStaff
    {
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Name { get; set; }
        public string Gate { get; set; } // Example: Gatekeeper, Monitor, etc.
        public string ContactNumber { get; set; }
        public string Password { get; set; }
    }
}