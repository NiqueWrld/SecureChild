namespace SecureChild.Models
{
    public class Alert
    {
        public int Id { get; set; }

        // Alert message content
        public string Message { get; set; }

        // Time the alert was sent
        public DateTime Time { get; set; }

        // Type of alert (Entry, Exit, Emergency)
        public string AlertType { get; set; }

        // Foreign key to the student (if it's a student alert)
        public int StudentId { get; set; }
        public Student Student { get; set; }

        // Flag for whether the alert was sent successfully
        public bool SentSuccessfully { get; set; }
    }
}