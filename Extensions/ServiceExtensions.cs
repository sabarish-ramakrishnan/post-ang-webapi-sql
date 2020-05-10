using Microsoft.Extensions.DependencyInjection;
using post_ang_webapi_sql.Services;

namespace post_ang_webapi_sql.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
    }
}