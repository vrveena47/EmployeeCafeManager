using Autofac;
using Autofac.Extensions.DependencyInjection;
using CafeEmployeeManager.Server.API.Helpers;
using CafeEmployeeManager.Server.Application.Commands.Cafe;
using CafeEmployeeManager.Server.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Use Autofac as DI container
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    // Register MediatR handlers manually
    container.RegisterAssemblyTypes(typeof(CreateCafeCommandHandler).Assembly)
             .AsImplementedInterfaces(); // This registers IRequestHandler and INotificationHandler

    // Register the Mediator itself
    container.RegisterType<Mediator>().As<IMediator>().InstancePerLifetimeScope();

    // Optional: register other dependencies
    container.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
             .AsImplementedInterfaces();
});

// Register DbContext normally
builder.Services.AddDbContext<CafeEmployeeDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy
            .WithOrigins("http://localhost:56544") // your React app
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// Controllers, Swagger, etc.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();

// Use CORS
app.UseCors("AllowFrontend");
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllers();


app.Run();
