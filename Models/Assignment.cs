using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace vrwebapi.Models
{
    public class Assignment
    {
        [Key]
        public int aid { get; set; }

        public int courseid { get; set; }

        [ForeignKey("courseid")]
        public Course? course { get; set; }

        public int classid { get; set; }

        [ForeignKey("classid")]
        public Classes? Classes { get; set; }

        public string aname { get; set; }

        public string dated { get; set; }

        public string duedate { get; set; }

        public TimeOnly time { get; set; }

        public string file { get; set; }

        public string description { get; set; }
    }
}
