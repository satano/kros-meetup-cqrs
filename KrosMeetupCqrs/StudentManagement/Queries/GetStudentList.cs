using MediatR;
using StudentManagement.Data;

namespace StudentManagement.Queries
{
    internal sealed class GetStudentList
    {
        internal sealed class Query : IRequest<IEnumerable<Student>>
        {
            public Query(string enrolledIn)
            {
                EnrolledIn = enrolledIn;
            }

            public string EnrolledIn { get; }
        }

        internal sealed class Handler : IRequestHandler<Query, IEnumerable<Student>>
        {
            private readonly KrosMeetupCqrsContext _dbContext;

            public Handler(KrosMeetupCqrsContext dbContext)
            {
                _dbContext = dbContext;
            }

            public Task<IEnumerable<Student>> Handle(Query request, CancellationToken cancellationToken)
            {
                var studentRepository = new StudentRepository(_dbContext);
                return Task.FromResult(studentRepository.GetList(request.EnrolledIn));
            }
        }
    }
}
