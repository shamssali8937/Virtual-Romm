using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vrwebapi.Models
{
    public class Student
    {
        [Key]
        public int studentid { get; set; }

        public int userid { get; set; }

        [ForeignKey("userid")]
        public User? user { get; set; }

        public string rollno { get; set; }

        public float cgpa { get; set; }

        public string program   { get; set; }
    }
}
