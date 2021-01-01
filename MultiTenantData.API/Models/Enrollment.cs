using System;

namespace MultiTenantData.API.Models
{
    public class Enrollment
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public Guid StudentId { get; set; }
        public Grade Grade { get; set; }
        public Course Course { get; set; }
        public Student Student { get; set; }
    }

    public enum Grade
    {
        A, B, C, D, E
    }
}