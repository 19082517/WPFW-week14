using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WPFW_week14.Models
{
    public class StudentCourse
    {
        public int Id { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
