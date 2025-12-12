
using Application.Abstractions.CQRS;
using Application.Abstractions.Persistence;
using Application.Abstractions.Services;
using Infrastructure.Auditing;
using Infrastructure.CQRS;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Identity;
using Application;

namespace Infrastructure
{

    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? "Data Source=dotnetassessment.db";

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(connectionString));

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            // CQRS dispatchers
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();

            // Services
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IAuditService, DatabaseAuditService>();
            services.AddScoped<ICurrentUser, HttpContextCurrentUser>();
            services.AddScoped<Application.Abstractions.Services.IJwtTokenService, Identity.JwtTokenService>();

            // Database Seeder
            services.AddScoped<Persistence.DatabaseSeeder>();

            // Application registrations (handlers, validators)
            services.AddApplication();

            return services;
        }
    }
}