using System;

namespace RPA_Alura
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _dbContext;

        public CourseService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveCourse(Course course)
        {
            _dbContext.Courses.Add(course);
            _dbContext.SaveChanges();
        }
    }
}
