using ApplicantManagement.Domain.Entities;
using ApplicantManagement.Domain.Interfaces;
using ApplicantManagement.Infrastructure.Data;
using ApplicantManagement.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicantManagement.Infrastructure;

public static class DI
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicantDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DevConnection"),
                b => b.MigrationsAssembly(typeof(ApplicantDbContext).Assembly.FullName)));

        services.AddScoped<IRepository<Applicant>, Repository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}