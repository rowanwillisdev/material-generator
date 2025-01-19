using Microsoft.Extensions.DependencyInjection;
using RowanWillis.Common.Application;

namespace MaterialGenerator.Application;

public static class Module
{
    public static IServiceCollection AddApplicationModule(this IServiceCollection services) => services
        .AddScoped<IUnitOfWork, TrivialUnitOfWork>()
        .AddHandlersFromAssembly(typeof(Module).Assembly);
}