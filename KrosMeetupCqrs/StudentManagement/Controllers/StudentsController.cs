using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentManagement.Commands;
using StudentManagement.Data;
using StudentManagement.Dto;
using StudentManagement.Queries;

namespace StudentManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly KrosMeetupCqrsContext _dbContext;
        private readonly StudentRepository _studentRepository;
        private readonly CourseRepository _courseRepository;

        public StudentsController(IMediator mediator, KrosMeetupCqrsContext dbContext)
        {
            _mediator = mediator;
            _dbContext = dbContext;
            _studentRepository = new StudentRepository(_dbContext);
            _courseRepository = new CourseRepository(_dbContext);
        }

        [HttpGet]
        public async Task<IEnumerable<StudentDto>> GetList(string enrolledIn)
            => (await _mediator.Send(new GetStudentList.Query(enrolledIn))).Select(s => ConvertToDto(s));

        [HttpGet("{id}")]
        public async Task<StudentDto> Get(int id)
            => ConvertToDto(await _mediator.Send(new GetStudent.Query(id)));

        [HttpPost]
        public IActionResult Register([FromBody] NewStudentDto dto)
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

        [HttpPost("{id}/enrollments")]
        public IActionResult Enroll(int id, [FromBody] StudentEnrollDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student is null)
            {
                return BadRequest($"No student found with ID {id}.");
            }
            Course course = _courseRepository.GetByName(dto.Course);
            if (course is null)
            {
                return BadRequest($"No course found with name {dto.Course}.");
            }

            student.Enroll(course);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpDelete("{id}/enrollments/{enrollmentNumber}")]
        public IActionResult Disenroll(int id, int enrollmentNumber)
        {
            Student student = _studentRepository.GetById(id);
            if (student is null)
            {
                return BadRequest($"No student found with ID {id}.");
            }
            enrollmentNumber--;
            Enrollment enrollment = student.GetEnrollment(enrollmentNumber); ;
            if (enrollment is null)
            {
                return BadRequest($"No enrollment with number {enrollmentNumber} found.");
            }

            student.RemoveEnrollment(enrollment);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}/enrollments/{enrollmentNumber}")]
        public IActionResult ChangeEnrollment(int id, int enrollmentNumber, [FromBody] StudentChangeEnrollmentDto dto)
        {
            Student student = _studentRepository.GetById(id);
            if (student is null)
            {
                return BadRequest($"No student found with ID {id}.");
            }
            Course course = _courseRepository.GetByName(dto.Course);
            if (course is null)
            {
                return BadRequest($"No course found with name {dto.Course}.");
            }
            enrollmentNumber--;
            Enrollment enrollment = student.GetEnrollment(enrollmentNumber); ;
            if (enrollment is null)
            {
                return BadRequest($"No enrollment with number {enrollmentNumber} found.");
            }

            student.RemoveEnrollment(enrollment);
            student.Enroll(course);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult EditPersonalInfo(int id, [FromBody] StudentPersonalInfoDto dto)
        {
            var command = new EditPersonalInfoCommand(id, dto.Name, dto.Email);
            _mediator.Send(command);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Unregister(int id)
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
