using StudentManagement.Data;

namespace StudentManagement.Commands
{
    public class EditPersonalInfoCommand
    {
        public EditPersonalInfoCommand(int id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        public int Id { get; }
        public string Name { get; }
        public string Email { get; }
    }

    public class EditPersonalInfoCommandHandler
    {
        private readonly KrosMeetupCqrsContext _dbContext;

        public EditPersonalInfoCommandHandler(KrosMeetupCqrsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Handle(EditPersonalInfoCommand command)
        {
            var studentRepository = new StudentRepository(_dbContext);
            Student student = studentRepository.GetById(command.Id);
            if (student is null)
            {
                throw new BadHttpRequestException($"No student found with ID {command.Id}.");
            }

            student.Name = command.Name;
            student.Email = command.Email;
            _dbContext.SaveChanges();
        }
    }
}
