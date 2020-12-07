using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WPFW_week14.Models;

namespace WPFW_week14.Data
{
    public class WPFW_week14Context : DbContext
    {
        public WPFW_week14Context (DbContextOptions<WPFW_week14Context> options)
            : base(options)
        {
        }

        public DbSet<WPFW_week14.Models.Student> Student { get; set; }

        public DbSet<WPFW_week14.Models.Course> Course { get; set; }

        public DbSet<WPFW_week14.Models.StudentCourse> StudentCourse { get; set; }
    }
}
