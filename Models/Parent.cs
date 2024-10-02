namespace SecureChild.Models
{
    public class Parent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [StartsWith("27")]
        public string ContactInfo { get; set; }
        public string apiKey { get; set; }

        // A parent can have multiple students
        public ICollection<Student> Students { get; set; }

        public Parent()
        {
            Students = new List<Student>();
        }
    }
}