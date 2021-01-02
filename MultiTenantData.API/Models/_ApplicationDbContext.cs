using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MultiTenantData.API.Util.Security;

namespace MultiTenantData.API.Models
{
    public class ApplicationDbContext: DbContext
    {
        private readonly IClaimsProvider _claimsProvider;
        private int _userId => _claimsProvider.UserId;
        private IEnumerable<Guid> _acessibleSchoolIds => _claimsProvider.AcessibleSchoolIds; 

        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<School> Schools { get; set; }
        public DbSet<Student> Students { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IClaimsProvider claimsProvider) : base(options)
        {
            _claimsProvider = claimsProvider;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasQueryFilter(x => _acessibleSchoolIds.Contains(x.School.Id));
                entity.ToTable("Course");
            });

            modelBuilder.Entity<Enrollment>(entity =>
            {
                entity.HasQueryFilter(x => _acessibleSchoolIds.Contains(x.Course.School.Id));
                entity.ToTable("Enrollment");
            });

            modelBuilder.Entity<School>(entity =>
            {
                entity.HasQueryFilter(x => _acessibleSchoolIds.Contains(x.Id));
                entity.ToTable("School");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasQueryFilter(x => x.Enrollments.Any(xx => _acessibleSchoolIds.Contains(xx.Course.School.Id)));
                entity.ToTable("Student");
            });
        }
    }
}