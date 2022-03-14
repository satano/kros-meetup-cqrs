using MediatR;
using StudentManagement.Data;

namespace StudentManagement.Commands
{
    public class EditPersonalInfoCommandHandler : IRequestHandler<EditPersonalInfoCommand>
    {
        private readonly KrosMeetupCqrsContext _dbContext;

        public EditPersonalInfoCommandHandler(KrosMeetupCqrsContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Unit> Handle(EditPersonalInfoCommand request, CancellationToken cancellationToken)
        {
            var studentRepository = new StudentRepository(_dbContext);
            Student student = studentRepository.GetById(request.Id);
            if (student is null)
            {
                throw new BadHttpRequestException($"No student found with ID {request.Id}.");
            }

            student.Name = request.Name;
            student.Email = request.Email;
            _dbContext.SaveChanges();
            return Unit.Task;
        }
    }
}
