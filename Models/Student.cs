namespace SecureChild.Models
{
        public class Student
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Age { get; set; }

            // Foreign key relationship to the Parent class
            public int ParentId { get; set; }
            public Parent Parent { get; set; }

            // Tracking student's entry and exit
            public ICollection<Entry> Entries { get; set; }
            public ICollection<Exit> Exits { get; set; }

            public Student()
            {
                Entries = new List<Entry>();
                Exits = new List<Exit>();
            }
        }
}