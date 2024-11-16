using System.ComponentModel.DataAnnotations;

namespace vrwebapi.Models
{
    public class User
    {
        [Key]
        public int userid { get; set; }

        public  string Name { get; set; }

        public string username { get; set; }

        public  string Email { get; set; }

        public  string Password { get; set; }
    }
}
