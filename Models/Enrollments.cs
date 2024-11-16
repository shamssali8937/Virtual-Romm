using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vrwebapi.Models
{
    public class Enrollments
    {
        [Key]

        public int enrollmentid { get; set; }

        public int courseid { get; set; }

        [ForeignKey("courseid")]
        public Course? course { get; set; }

        public int classid { get; set; }

        [ForeignKey("classid")]
        public Classes? classes { get; set; }

        public int studentid { get; set; }

        [ForeignKey("studentid")]
        public Student? student { get; set; }
    }
}
