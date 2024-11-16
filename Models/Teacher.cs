using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vrwebapi.Models
{
    public class Teacher
    {

        [Key]

        public int teacherid { get; set; }

        public int userid { get; set; }

        [ForeignKey("userid")]
        public User? user { get; set; }

        public string department {  get; set; }
    }
}
