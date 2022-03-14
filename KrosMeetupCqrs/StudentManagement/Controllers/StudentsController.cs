using Microsoft.AspNetCore.Mvc;
using StudentManagement.Data;
using StudentManagement.Dto;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly KrosMeetupCqrsContext _dbContext;
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentsController(KrosMeetupCqrsContext dbContext)
        {
            _dbContext = dbContext;
            _studentRepository = new StudentRepository(_dbContext);
            _courseRepository = new CourseRepository(_dbContext);
        }

        [HttpGet]
        public IEnumerable<StudentDto> GetList(string enrolledIn)
            => _studentRepository.GetList(enrolledIn).Select(s => ConvertToDto(s));

        [HttpGet("{id}")]
        public StudentDto Get(int id)
            => ConvertToDto(_studentRepository.GetById(id));

        [HttpPost]
        public IActionResult Create([FromBody] StudentDto dto)
        {
            var student = new Student
            {
                Name = dto.Name,
                Email = dto.Email
            };

            if (dto.Course1 != null)
            {
                Course course = _courseRepository.GetByName(dto.Course1);
                student.Enroll(course);
            }

            if (dto.Course2 != null)
            {
                Course course = _courseRepository.GetByName(dto.Course2);
                student.Enroll(course);
            }

            _studentRepository.Save(student);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] StudentDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student is null)
            {
                return BadRequest($"No student found with ID {id}.");
            }

            student.Name = dto.Name;
            student.Email = dto.Email;

            Enrollment firstEnrollment = student.Enrollment1;
            Enrollment secondEnrollment = student.Enrollment2;

            if (HasEnrollmentChanged(dto.Course1, firstEnrollment))
            {
                if (string.IsNullOrWhiteSpace(dto.Course1)) // Student disenrolls
                {
                    student.RemoveEnrollment(firstEnrollment);
                }
                else
                {
                    Course course = _courseRepository.GetByName(dto.Course1);
                    if (firstEnrollment is not null)
                    {
                        student.RemoveEnrollment(firstEnrollment);
                    }
                    student.Enroll(course);
                }
            }

            if (HasEnrollmentChanged(dto.Course2, secondEnrollment))
            {
                if (string.IsNullOrWhiteSpace(dto.Course2)) // Student disenrolls
                {
                    student.RemoveEnrollment(secondEnrollment);
                }
                else
                {
                    Course course = _courseRepository.GetByName(dto.Course2);
                    if (secondEnrollment is not null)
                    {
                        student.RemoveEnrollment(secondEnrollment);
                    }
                    student.Enroll(course);
                }
            }

            _dbContext.SaveChanges();
            return Ok();
        }

        private bool HasEnrollmentChanged(string newCourseName, Enrollment enrollment)
        {
            if (string.IsNullOrWhiteSpace(newCourseName) && enrollment == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(newCourseName) || enrollment == null)
            {
                return true;
            }

            if (newCourseName is not null)
            {
                return !newCourseName.Equals(enrollment.Course.Name, StringComparison.OrdinalIgnoreCase);
            }
            return true;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Student student = _studentRepository.GetById(id);
            if (student is null)
            {
                return BadRequest($"No student found with ID {id}.");
            }

            _studentRepository.Delete(student);
            _dbContext.SaveChanges();
            return Ok();
        }

        private StudentDto ConvertToDto(Student student)
        {
            return student is null
                ? null
                : new StudentDto
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    Course1 = student.Enrollment1?.Course?.Name,
                    Course2 = student.Enrollment2?.Course?.Name
                };
        }
    }
}
