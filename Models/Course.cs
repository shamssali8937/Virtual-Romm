using System.ComponentModel.DataAnnotations;

namespace vrwebapi.Models
{
    public class Course
    {
        [Key]

        public int courseid { get; set; }

        public  string coursename { get; set; }

        public  string description { get; set; }
    }
}
