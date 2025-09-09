using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Infrastructure.InfrastructureBase;
using InternetBanking.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace InternetBanking.Infrastructure
{
    public static class ModelInfrastructureDependecies
    {
        public static IServiceCollection addInfrastructureDependecies(this IServiceCollection services)
        {
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            return services;
        }
    }
}
