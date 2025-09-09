using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.InfrastructureBase;


namespace InternetBanking.Infrastructure.Abstract
{
    public interface IAccountRepository:IGenericRepository<Account>
    {
        public Task<Account?> GetByIdWithTransactionsAsync(int id);
        public Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default);
        public Task<List<Account>> GetAllAsync();
        public Task<List<Account>> GetByUserIdAsync(string userId, CancellationToken ct = default);


    }
}

