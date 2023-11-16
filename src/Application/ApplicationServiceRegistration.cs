using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Application.Behaviors;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(Assembly.GetExecutingAssembly());

        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnHandledExceptionBehaviour<,>));
        //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        return services;
    }
}
