using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using vrwebapi.Data;
using vrwebapi.Models;


namespace vrwebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VirtualController : ControllerBase
    {
        private readonly vrdbcontext dbcontext;

        private readonly IConfiguration configuration;

        public VirtualController(vrdbcontext dbcontext, IConfiguration configuration)
        {
            this.dbcontext = dbcontext;
            this.configuration = configuration;
        }

        [HttpPost("Register")]

        public Response Register([FromBody] User model)
        {
            //string salt = BCrypt.Net.BCrypt.GenerateSalt();
            Response response = new Response();
            bool check = true;
            if (check == dbcontext.users.Any(e => e.Email == model.Email))
            {
                response.statuscode = 400;
                response.statusmessage = "Email is Already Taken";
                return response;
            }
            if (check == dbcontext.users.Any(e => e.username == model.username))
            {
                response.statuscode = 400;
                response.statusmessage = "Username is Already Taken";
                return response;
            }


            User user = new User
            {
                Name = model.Name,
                username = model.username,
                Email = model.Email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password, 13)
                //  Password = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password + salt, 13)
            };

            dbcontext.users.Add(user);
            dbcontext.SaveChanges();

            response.statuscode = 200;
            response.statusmessage = "Registered";
            response.user = user;
            return response;

        }

        [HttpPost("Login")]

        public Response Login([FromBody] Login model)
        {
            Response response = new Response();

            var user = dbcontext.users.FirstOrDefault(e => e.Email == model.Email);

            if (user == null)
            {
                response.statuscode = 400;
                response.statusmessage = "Error";
                return response;
            }
            else
            {
                if (BCrypt.Net.BCrypt.EnhancedVerify(model.Password, user.Password) && user.Email == model.Email)
                {
                    response.statuscode = 200;
                    response.statusmessage = jwttoken(model);
                    return response;

                }
                else
                {
                    response.statuscode = 100;
                    response.statusmessage = "Invalid password";

                }
                return response;
            }
        }

        private string jwttoken(Login login)
        {
            var claims = new[]
            {
        new Claim(JwtRegisteredClaimNames.Sub, configuration["Jwt:Subject"]),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("email", login.Email)
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [Authorize]
        [HttpGet("Classlit")]

        public Response Classlist()
        {

            Response response = new Response();

            ClaimsPrincipal userclaims = User;
            var identity = userclaims.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var claim = identity.Claims;
                var email = claim.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (email != null)
                {
                    int studentid = dbcontext.students.Include(u => u.user).Where(u => u.user.Email == email.Value).Select(s => s.studentid).FirstOrDefault();

                    if (studentid != 0)
                    {
                        var list = dbcontext.enrollments.Include(c => c.classes).Where(e => e.studentid == studentid).Select(c => c.classes).ToList();
                        if (list != null)
                        {
                            response.statuscode = 200;
                            response.statusmessage = "found";
                            response.classes = list;
                            return response;
                        }
                        else
                        {
                            response.statuscode = 100;
                            response.statusmessage = "no class found";
                            return response;
                        }
                    }
                    else
                    {
                        response.statuscode = 100;
                        response.statusmessage = "no student exist";
                        return response;
                    }

                }
                else
                {
                    response.statuscode = 100;
                    response.statusmessage = "no user exist";
                    return response;
                }
            }
            else
            {
                response.statuscode = 100;
                response.statusmessage = "error";
                return response;
            }
        }

        [Authorize]
        [HttpGet("Getuser")]

        public Response Getuser()
        {
            Response response = new Response();
            ClaimsPrincipal userclaim = User;

            var identity = userclaim.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var claim = identity.Claims;
                var email = claim.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (email != null)
                {
                    var user = dbcontext.users.FirstOrDefault(u => u.Email == email.Value);
                    if (user != null)
                    {
                        response.statuscode = 200;
                        response.statusmessage = "found";
                        response.user = user;
                        return response;
                    }
                    else
                    {
                        response.statuscode = 100;
                        response.statusmessage = "No user";
                        return response;
                    }

                }
            }
            response.statuscode = 100;
            response.statusmessage = "error";
            return response;
        }

        [Authorize]
        [HttpPost("Addcourse")]

        public Response Addcourse([FromBody] Course model)
        {
            Response response = new Response();

            var course = new Course
            {
                coursename = model.coursename,
                description = model.description
            };

            dbcontext.courses.Add(course);
            dbcontext.SaveChanges();


            response.statuscode = 200;
            response.statusmessage = "Course Added";
            return response;

        }

        [Authorize]
        [HttpGet("Courselist")]

        public Response Courselist()
        {
            Response response = new Response();

            var courses = dbcontext.courses.ToList();

            if (courses != null)
            {
                response.statuscode = 200;
                response.statusmessage = "Course List";
                response.courses = courses;
                return response;
            }
            else
            {
                response.statuscode = 100;
                response.statusmessage = "No Course Found";
                return response;
            }
        }

        [HttpPost("Addclass")]

        public Response Addclass([FromBody] Classes model)
        {
            Response response = new Response();

            if (model.courseid == 0 || string.IsNullOrEmpty(model.classname))
            {
                response.statuscode = 400;
                response.statusmessage = "Invalid data";
                return response;
            }

            var classes = new Classes
            {
                classname = model.classname,
                courseid = model.courseid,
                description = model.description
            };

            dbcontext.classes.Add(classes);
            dbcontext.SaveChanges();
            response.statuscode = 200;
            response.statusmessage = "class added";
            return response;

        }

        [HttpPost("Join")]

        public Response Join([FromBody] Enrollments model)
        {
            Response response = new Response();
            if (model.classid == 0)
            {
                response.statuscode = 400;
                response.statusmessage = "Invalid data";
                return response;
            }
            bool alreadyexisted = dbcontext.enrollments.Any(s => s.studentid == model.studentid && s.classid == model.classid);
            if (alreadyexisted)
            {
                response.statuscode = 400;
                response.statusmessage = "Same studentd already joined";
                return response;
            }
            Enrollments join = new Enrollments
            {
                classid = model.classid,
                courseid = model.courseid,
                studentid = model.studentid,
            };

            dbcontext.enrollments.Add(join);
            dbcontext.SaveChanges();

            response.statuscode = 200;
            response.statusmessage = "JOINED";
            return response;

        }

        [HttpPost("Addteacher")]

        public Response Addteacher([FromBody] Teacher model)
        {
            Response response = new Response();
            bool existingstudent = dbcontext.students.Any(s => s.userid == model.userid);
            if (existingstudent)
            {
                response.statuscode = 400;
                response.statusmessage = "user exist as a student";
                return response;
            }
            Teacher teacher = new Teacher
            {
                userid = model.userid,
                department = model.department
            };
            dbcontext.teachers.Add(teacher);
            dbcontext.SaveChanges();

            response.statuscode = 200;
            response.statusmessage = "Added";
            return response;

        }

        [HttpPost("Addstudent")]

        public Response Addstudent([FromBody] Student model)
        {
            Response response = new Response();
            bool existingteacher = dbcontext.teachers.Any(t => t.userid == model.userid);
            if (existingteacher)
            {
                response.statuscode = 400;
                response.statusmessage = "user exist as a teacher";
                return response;
            }
            Student student = new Student
            {
                userid = model.userid,
                cgpa = model.cgpa,
                program = model.program,
                rollno = model.rollno
            };
            dbcontext.students.Add(student);
            dbcontext.SaveChanges();

            response.statuscode = 200;
            response.statusmessage = "Added";
            return response;

        }

        [HttpPost("Getuserid")]

        public Response GetUserID([FromBody] Name model)
        {
            Response response = new Response();
            var user = dbcontext.users.FirstOrDefault(u => u.Name == model.name);
            if (user != null)
            {
                response.statuscode = 200;
                response.user = user;
                return response;
            }
            else
            {
                response.statuscode = 404;
                response.statusmessage = "User not found";
                return response;
            }
        }

        [HttpPost("studentid")]

        public Response studentid([FromBody] Name model)
        {
            Response response = new Response();
            int student = dbcontext.students.Include(u => u.user).Where(u => u.user.Name == model.name).Select(s => s.studentid).FirstOrDefault();
            if (student != 0)
            {
                response.statuscode = 200;
                response.statusmessage = student.ToString();
                return response;
            }
            else
            {
                response.statuscode = 100;
                response.statusmessage = "not found";
                return response;
            }

        }

        [HttpPost("Getcourse")]
        public Response Getcourse([FromBody] Classid model)
        {
            Response response = new Response();
            int course = dbcontext.classes.Include(c => c.course).Where(cl => cl.classid == model.classid).Select(c => c.courseid).FirstOrDefault();
            if (course != 0)
            {
                response.statuscode = 200;
                response.statusmessage = course.ToString();
                return response;
            }
            else
            {
                response.statuscode = 400;
                response.statusmessage = "not found";
                return response;
            }
        }

        [HttpPost("courseid")]

        public Response courseid([FromBody] Name model)
        {
            Response response = new Response();
            int course = dbcontext.courses.Where(c => c.coursename == model.name).Select(c => c.courseid).FirstOrDefault();
            if (course != 0)
            {
                response.statuscode = 200;
                response.statusmessage = course.ToString();
                return response;
            }
            else
            {
                response.statuscode = 100;
                response.statusmessage = "not found";
                return response;
            }

        }

        [HttpPost("classid")]

        public Response classid([FromBody] Name model)
        {
            Response response = new Response();
            int clid = dbcontext.classes.Where(c => c.classname == model.name).Select(c => c.classid).FirstOrDefault();
            if (clid != 0)
            {
                response.statuscode = 200;
                response.statusmessage = clid.ToString();
                return response;
            }
            else
            {
                response.statuscode = 100;
                response.statusmessage = "not found";
                return response;
            }

        }

        [HttpPost("Addassignment")]

        public Response Addassignment([FromBody] Assignment model)
        {
            Response response = new Response();
            Assignment assignment = new Assignment
            {
                courseid = model.courseid,
                classid = model.classid,
                aname = model.aname,
                dated = model.dated,
                duedate = model.duedate,
                time = model.time,
                description = model.description
            };

            dbcontext.assignments.Add(assignment);
            dbcontext.SaveChanges();

            response.statuscode = 200;
            response.statusmessage = "Assignment Assigned To The Class";
            return response;

        }

        [Authorize]
        [HttpGet("Isteacher")]

        public Response Isteacher()
        {
            Response response = new Response();
            ClaimsPrincipal userclaim = User;

            var identity = userclaim.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var claims = identity.Claims;
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (email != null)
                {
                    int teacher = dbcontext.teachers.Include(u => u.user).Where(u => u.user.Email == email.Value).Select(t => t.teacherid).FirstOrDefault();
                    if (teacher != 0)
                    {
                        bool isteacher = true;
                        response.statuscode = 200;
                        response.statusmessage = isteacher.ToString();
                        return response;
                    }
                    else
                    {
                        response.statuscode = 400;
                        response.statusmessage = "no teacher exist";
                        return response;
                    }
                }
                else
                {
                    response.statuscode = 400;
                    response.statusmessage = "no user exist";
                    return response;
                }
            }
            else
            {
                response.statuscode = 100;
                response.statusmessage = "error";
                return response;
            }

        }

        [HttpPost("Teacherid")]

        public Response Teacherid([FromBody] Name model)
        {
            Response response = new Response();
            int student = dbcontext.teachers.Include(u => u.user).Where(u => u.user.Name == model.name).Select(t => t.teacherid).FirstOrDefault();
            if (student != 0)
            {
                response.statuscode = 200;
                response.statusmessage = student.ToString();
                return response;
            }
            else
            {
                response.statuscode = 100;
                response.statusmessage = "not found";
                return response;
            }

        }

        [HttpPost("teacherjoin")]

        public Response teacherjoin([FromBody] Teacherassigned model)
        {
            Response response = new Response();
            bool alreadyexisted = dbcontext.teacherassigneds.Any(t => t.teacherid == model.teacherid && t.classid == model.classid);
            if (alreadyexisted)
            {
                response.statuscode = 400;
                response.statusmessage = "Same teacher already joined";
                return response;
            }
            Teacherassigned teacher = new Teacherassigned
            {
                classid = model.classid,
                teacherid = model.teacherid


            };

            dbcontext.teacherassigneds.Add(teacher);
            dbcontext.SaveChanges();

            response.statuscode = 200;
            response.statusmessage = "joined";
            return response;

        }

        [Authorize]
        [HttpGet("Teacherclasses")]

        public Response Teacherclasses()
        {
            Response response = new Response();

            ClaimsPrincipal userclaims = User;
            var identity = userclaims.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var claim = identity.Claims;
                var email = claim.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                if (email != null)
                {
                    int studentid = dbcontext.teachers.Include(u => u.user).Where(u => u.user.Email == email.Value).Select(t => t.teacherid).FirstOrDefault();

                    if (studentid != 0)
                    {
                        var list = dbcontext.teacherassigneds.Include(c => c.classes).Where(t => t.teacherid == studentid).Select(c => c.classes).ToList();
                        if (list != null)
                        {
                            response.statuscode = 200;
                            response.statusmessage = "found";
                            response.classes = list;
                            return response;
                        }
                        else
                        {
                            response.statuscode = 100;
                            response.statusmessage = "no class found";
                            return response;
                        }
                    }
                    else
                    {
                        response.statuscode = 100;
                        response.statusmessage = "no Teacher exist";
                        return response;
                    }

                }
                else
                {
                    response.statuscode = 100;
                    response.statusmessage = "no user exist";
                    return response;
                }
            }
            else
            {
                response.statuscode = 100;
                response.statusmessage = "error";
                return response;
            }
        }

        [HttpPost("GetAssignment")]

        public Response GetAssignments([FromBody] Name model)
        {
            Response response = new Response();
            var list = dbcontext.assignments.Include(cl => cl.Classes).Where(cl => cl.Classes.classname == model.name).ToList();
            if(list!=null)
            {
                response.statuscode = 200;
                response.statusmessage = "Assignments";
                response.assignment = list;
                return response;
            }
            else
            {
                response.statuscode = 400;
                response.statusmessage = "Assignments Not Found";
                return response;
            }
        }

        [HttpPost("submit")]

        public Response submit([FromBody] Submission model)
        {
            Response response = new Response();
            var submission = new Submission { 
                aid=model.aid,
                studentid=model.studentid,
                description=model.description,
                file=model.file
            };

            dbcontext.submissions.Add(submission);
            dbcontext.SaveChanges();

            response.statuscode = 200;
            response.statusmessage = "Submitted";
            return response;
        }

        [HttpPost("SubmissionList")]

        public Response SubmissionList([FromBody] Name model)
        {
            Response response = new Response();
            var list = dbcontext.submissions.Include(a => a.assignment).Where(a => a.assignment.Classes.classname == model.name).Select(a => new submissiondetail
            {
                Assignment = a.assignment,
                submissionid = a.sid,
                student = a.student.user.Name
            }).ToList();

            if(list!=null)
            {
                response.statuscode = 200;
                response.statusmessage = "found";
                response.submission = list;
                return response;
            }
            else
            {
                response.statuscode = 400;
                response.statusmessage = "not found";
                return response;
            }

        }

    }
}