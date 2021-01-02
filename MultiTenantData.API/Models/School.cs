using System;
using System.Collections.Generic;

namespace MultiTenantData.API.Models
{
    public class School
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}