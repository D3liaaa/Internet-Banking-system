using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.InfrastructureBase;

namespace InternetBanking.Infrastructure.Abstract
{
    public interface ITransactionRepository: IGenericRepository<Transaction>
    {
        public Task<List<Transaction>> GetByAccountIdAsync(int accountId);
        Task<List<Transaction>> GetStatementAsync(
            int accountId,
            DateTime from,
            DateTime to,
            CancellationToken ct = default);

    }
}
