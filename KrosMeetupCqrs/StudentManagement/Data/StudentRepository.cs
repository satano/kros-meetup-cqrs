using Microsoft.EntityFrameworkCore;

namespace StudentManagement.Data
{
    public class StudentRepository
    {
        private readonly KrosMeetupCqrsContext _dbContext;

        public StudentRepository(KrosMeetupCqrsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Student GetById(int id)
        {
            var student = _dbContext.Students.FirstOrDefault(s => s.Id == id);
            return student;
        }

        public IEnumerable<Student> GetList(string enrolledIn)
        {
            IQueryable<Student> query = _dbContext.Students;

            if (!string.IsNullOrWhiteSpace(enrolledIn))
            {
                query = query.Where(x => x.Enrollments.Any(e => e.Course.Name == enrolledIn));
            }

            List<Student> result = query.ToList();

            return result;
        }

        public void Save(Student student)
        {
            _dbContext.Students.Add(student);
        }

        public void Delete(Student student)
        {
            _dbContext.Students.Remove(student);
        }
    }
}
