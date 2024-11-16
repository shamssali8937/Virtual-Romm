using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vrwebapi.Models
{
    public class Classes
    {
        [Key]

        public int classid { get; set; }

        public int courseid { get; set; }

        [ForeignKey("courseid")]
        public Course? course { get; set; }

        public string classname { get; set; }

        public string description { get; set; }
    }
}
