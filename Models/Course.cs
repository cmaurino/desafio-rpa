using System.ComponentModel.DataAnnotations;

namespace RPA_Alura
{
    public class Course
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Professor { get; set; }

        public string Duration { get; set; }

        public string Description { get; set; }
    }
}
