using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vrwebapi.Data;
using vrwebapi.Models;
using vrwebapi.UploadModels;

namespace vrwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class V2Controller : ControllerBase
    {
        private readonly vrdbcontext dbcontext;

        public V2Controller(vrdbcontext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

      
        [HttpPut("updateassignment/{assignmentid}")]

        public Response Response([FromRoute]int assignmentid,[FromForm] Assignmentupload assignment)
        {     
            Response response = new Response();

            string mainpath = Path.Combine("wwwroot", "UploadedFiles");
            string uploadsfolder = Path.Combine(Directory.GetCurrentDirectory(), mainpath);

            if (!Directory.Exists(uploadsfolder))
            {
                Directory.CreateDirectory(uploadsfolder);
            }
            if(assignment.file == null || assignment.file.ContentType != "application/pdf")
            {
                response.statuscode = 101;
                response.statusmessage = "please Upload pdf only";
                return response;
            }
            string filename = assignment.aname.Replace(" ", "_") + ".pdf";
            string filepath = Path.Combine(uploadsfolder,filename);
            string path = Path.Combine("UploadedFiles", filename);
            using (Stream stream = new FileStream(filepath, FileMode.Create))
            {
                assignment.file.CopyTo(stream);
            }



            var existingassignment = dbcontext.assignments.Find(assignmentid);
            if (existingassignment != null)
            {
                existingassignment.aname = assignment.aname;
                existingassignment.description = assignment.description;
                existingassignment.duedate = assignment.duedate;
                existingassignment.dated = assignment.dated;
                existingassignment.file = path;
                dbcontext.SaveChanges();

                response.statuscode = 200;
                response.statusmessage = "Assignment updated successfully";
                return response;
                    
              
            }
            else
            {
                response.statuscode = 404;
                response.statusmessage = "unsuccessfull";
                return response;
            }
        }

        [HttpDelete("deleteassignment")]

        public Response Deleteassignment([FromBody]int assignmentid)
        {
            Response response = new Response();
            var assignment = dbcontext.assignments.Find(assignmentid);
            if (assignment != null)
            {
                dbcontext.assignments.Remove(assignment);
                dbcontext.SaveChanges();
                response.statuscode = 200;
                response.statusmessage = "Assignment deleted successfully";
                return response;
            }
            else
            {
                response.statuscode = 404;
                response.statusmessage = "unsuccessfull";
                return response;
            }
        }
    }
}
