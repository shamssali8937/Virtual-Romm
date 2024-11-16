using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace vrwebapi.Models
{
    public class Grades
    {
        [Key]
        public int gid { get; set; }

        public int sid { get; set; }

        [ForeignKey("sid")]

        public Submission? submission { get; set; }

        public string grades { get; set; }

        public string comments { get; set; }


    }
}
