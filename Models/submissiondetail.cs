namespace vrwebapi.Models
{
    public class submissiondetail
    {
        
        public int submissionid { get; set; }

        public string aname { get; set; }
        public int assignmentid { get; set; }
        public string description { get; set; }
        public string file { get; set; }
        public string student { get; set; }
        public bool issubmit { get; set; }
    }
}
