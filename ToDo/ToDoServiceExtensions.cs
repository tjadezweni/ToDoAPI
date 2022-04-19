using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ToDo.Cache;
using ToDo.Contracts.Cache;
using ToDo.Contracts.Services;
using ToDo.Infrastructure;
using ToDo.Mappers;
using ToDo.Services;

namespace ToDo
{
    public static class ToDoServiceExtensions
    {
        public static void RegisterToDoDbContext(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("DBConnection"),
                    serverOptions =>
                    {
                        serverOptions.MigrationsAssembly("ToDo");
                    });
            });
        }

        public static void RegisterAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ToDoTaskMapperProfile>();
            });
        }

        public static void RegisterToDoServices(this IServiceCollection services)
        {
            services.AddScoped<IToDoTaskService, ToDoTaskService>();
        }

        public static void RegisterRedisCache(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var redisCacheSettings = new RedisCacheSettings();
            configuration.GetSection(nameof(RedisCacheSettings)).Bind(redisCacheSettings);
            services.AddSingleton(redisCacheSettings);

            if (!redisCacheSettings.Enabled)
            {
                return;
            }

            services.AddStackExchangeRedisCache(options => options.Configuration = redisCacheSettings.ConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
        }
    }
}
