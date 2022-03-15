using MediatR;
using System.Text.Json;

namespace StudentManagement.Data
{
    internal class RequestLoggerPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
         where TRequest : IRequest<TResponse>
    {
        private readonly ILogger _logger;

        public RequestLoggerPipelineBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation("*** START LOGGER {0}: {1}", request.GetType().FullName, JsonSerializer.Serialize(request));
            var result = next();
            _logger.LogInformation("*** END LOGGER {0}: {1}", request.GetType().FullName, JsonSerializer.Serialize(request));
            return result;
        }
    }
}
