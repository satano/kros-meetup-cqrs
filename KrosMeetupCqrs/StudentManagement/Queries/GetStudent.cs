using MediatR;
using StudentManagement.Data;

namespace StudentManagement.Queries
{
    internal sealed class GetStudent
    {
        internal sealed class Query : IRequest<Student>
        {
            public Query(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        internal sealed class Handler : IRequestHandler<Query, Student>
        {
            private readonly KrosMeetupCqrsContext _dbContext;

            public Handler(KrosMeetupCqrsContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<Student> Handle(Query request, CancellationToken cancellationToken)
            {
                var studentRepository = new StudentRepository(_dbContext);
                return Task.FromResult(studentRepository.GetById(request.Id));
            }
        }
    }
}
