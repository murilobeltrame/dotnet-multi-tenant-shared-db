using System;
using System.Collections.Generic;

namespace MultiTenantData.API.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}