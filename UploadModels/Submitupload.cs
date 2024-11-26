using System.ComponentModel.DataAnnotations.Schema;
using vrwebapi.Models;

namespace vrwebapi.UploadModels
{
    public class Submitupload
    {
        public int sid { get; set; }

        public int aid { get; set; }

        public int studentid { get; set; }
        
        public string description { get; set; }

        public bool issubmit { get; set; }

        public IFormFile file { get; set; }
    }
}
