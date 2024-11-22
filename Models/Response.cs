namespace vrwebapi.Models;

public class Response
{
    public int statuscode { get; set; }

    public string statusmessage { get; set; }

    public User? user { get; set; }

    public List<Course>? courses { get; set; }

    public List<Classes>? classes { get; set; }

    public List<Assignment>? assignment { get; set; }


}
