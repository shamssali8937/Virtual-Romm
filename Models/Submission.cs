using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace vrwebapi.Models
{
    public class Submission
    {
        [Key]
        public int sid { get; set; }

        public int aid { get; set; }

        [ForeignKey("aid")]
        public Assignment? assignment { get; set; }

        public int studentid { get; set; }

        [ForeignKey("studentid")]
        public Student? student { get; set; }

        public string description { get; set; }

        public bool issubmit { get; set; }

        public string file { get; set; }

    }
}
