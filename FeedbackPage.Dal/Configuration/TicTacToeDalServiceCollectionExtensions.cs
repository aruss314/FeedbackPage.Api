using FeedbackPage.Dal.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using System.Data;

namespace FeedbackPage.Dal.Configuration
{
    public static class TicTacToeDalServiceCollectionExtensions
    {
        public static IServiceCollection AddArussDbConnection(this IServiceCollection services, IConfiguration config)
        {
            services.TryAddTransient<IDbConnection>(sp => new NpgsqlConnection(config.GetSection("Psql_Connection").Value));
            return services;
        }
        public static IServiceCollection AddWinHistoryRepository(this IServiceCollection services, IConfiguration config)
        {
            services.AddArussDbConnection(config)
            .AddTransient<IWinHistoryRepository, WinHistoryRepository>();
            return services;
        }
    }
}
