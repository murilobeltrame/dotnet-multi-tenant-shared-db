using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;

namespace MultiTenantData.API.Models
{
    public static class DbInitializer
    {
        private static IEnumerable<Student> GetStudents(int qtd) {
            var studentsFaker = new Faker<Student>()
                .RuleFor(o => o.FirstName, f => f.Name.FirstName())
                .RuleFor(o => o.LastName, f => f.Name.LastName())
                .RuleFor(o => o.EnrollmentDate, f => f.Date.Recent(356));
            return studentsFaker.Generate(qtd);
        }

        private static IEnumerable<Course> GetCourses(int qtd) {
            var coursesFaker = new Faker<Course>()
                .RuleFor(o => o.Title, f => f.Lorem.Sentence(3))
                .RuleFor(o => o.Credits, f => f.Random.Int(100, 300));
            return coursesFaker.Generate(qtd);
        }

        private static int GetRandom(int min, int max) {
            var r = new Random();
            return r.Next(min, max);
        }

        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            var coursesQtd = 10;
            var courses = GetCourses(coursesQtd);
            context.Courses.AddRange(courses);

            var students = GetStudents(50);
            context.Students.AddRange(students);

            context.SaveChanges();

            foreach (var student in students)
            {
                var courseIdx = GetRandom(0, coursesQtd - 1);
                var gradeIdx = GetRandom(0, 4);

                var grade = ((Grade)gradeIdx);


                context.Enrollments.Add(new Enrollment
                {
                    Student = student,
                    Course = courses.ElementAt(courseIdx),
                    Grade = grade
                });
            }

            context.SaveChanges();
        }
    }
}