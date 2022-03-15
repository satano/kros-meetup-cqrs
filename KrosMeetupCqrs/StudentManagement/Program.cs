using MediatR;
using StudentManagement.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLogging();
builder.Services.AddControllers();
builder.Services.AddDbContext<KrosMeetupCqrsContext>();
builder.Services.AddMediatR(typeof(Program));
builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(RequestLoggerPipelineBehavior<,>));
builder.Services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(RequestMeasurePipelineBehavior<,>));

var app = builder.Build();


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
