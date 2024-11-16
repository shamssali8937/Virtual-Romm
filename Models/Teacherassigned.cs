using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vrwebapi.Models
{
    public class Teacherassigned
    {
        [Key]

        public int tsid { get; set; }

        public int classid { get; set; }

        [ForeignKey("classid")]
        public Classes? classes { get; set; }

        public int teacherid { get; set; }

        [ForeignKey("teacherid")]
        public Teacher? teacher { get; set; }
    }
}
