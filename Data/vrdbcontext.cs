using Microsoft.EntityFrameworkCore;
using vrwebapi.Models;

namespace vrwebapi.Data
{
    public class vrdbcontext:DbContext
    {
        public vrdbcontext(DbContextOptions<vrdbcontext>options):base(options)
        {
            
        }
        public DbSet<User> users { get; set; }

        public DbSet<Student> students { get; set; }    

        public DbSet<Teacher> teachers { get; set; }

        public DbSet<Course> courses { get; set; }

        public DbSet<Classes> classes { get; set; }

        public DbSet<Enrollments> enrollments { get; set; }

        public DbSet<Teacherassigned> teacherassigneds { get; set; }

        public DbSet<Assignment> assignments { get; set; }

        public DbSet<Submission> submissions { get; set; }

        public DbSet<Grades> grades { get; set; }


    }
}
