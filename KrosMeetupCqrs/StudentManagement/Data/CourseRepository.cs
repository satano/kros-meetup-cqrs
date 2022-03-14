namespace StudentManagement.Data
{
    public class CourseRepository
    {
        private readonly KrosMeetupCqrsContext _dbContext;

        public CourseRepository(KrosMeetupCqrsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Course GetByName(string courseName)
            => _dbContext.Courses.FirstOrDefault(course => course.Name == courseName);
    }
}
