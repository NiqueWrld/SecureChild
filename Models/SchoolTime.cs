using System.ComponentModel.DataAnnotations;

namespace SecureChild.Models
{
    public class SchoolTime
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; } = DateTime.Today;
        [DataType(DataType.Time)]
        public TimeSpan SchoolStartingTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan SchoolEndingTime { get; set; }
    }
}

