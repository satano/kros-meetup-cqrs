using MediatR;
using System.Diagnostics;

namespace StudentManagement.Data
{
    internal class RequestMeasurePipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public RequestMeasurePipelineBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("*** START MEASURE TIME");
            Stopwatch sw = Stopwatch.StartNew();
            var result = next();
            _logger.LogInformation("*** END MEASURE TIME: {0:mm':'ss'.'fff} ({1})", sw.Elapsed, request.GetType().FullName);
            return result;
        }
    }
}
