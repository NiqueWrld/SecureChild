namespace SecureChild.Models
{
    public class Entry
    {
        public int Id { get; set; }

        // Foreign key relationship to Student class
        public int StudentId { get; set; }
        public Student Student { get; set; }

        public int SecurityId { get; set; }

        public DateTime Time { get; set; } // Time of entry

        // Entry gate info if needed
        public string EntryGate { get; set; }
    }
}