using MediatR;

namespace StudentManagement.Commands
{
    public class EditPersonalInfoCommand : IRequest
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
}
