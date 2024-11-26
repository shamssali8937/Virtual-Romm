namespace vrwebapi.UploadModels
{
    public class Assignmentupload
    {
        public int courseid { get; set; }
        public int classid { get; set; }
        public string aname { get; set; }
        public string dated { get; set; }
        public string duedate { get; set; }
        public TimeOnly time { get; set; }
        public string description { get; set; }
        public IFormFile file { get; set; }
    }
}
