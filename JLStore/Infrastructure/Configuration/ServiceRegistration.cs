using AutoMapper;
using JLStore.Domain.Repositories;
using JLStore.Domain.Services;
using JLStore.Infrastructure.Repositories;
using JLStore.Mapping;

namespace JLStore.Infrastructure.Configuration;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton<TimeProvider>(TimeProvider.System);

        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerService, CustomerService>();

        services.AddAutoMapper(cfg => cfg.AddProfile<CustomerProfile>());

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
