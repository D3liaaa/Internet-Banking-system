
using Internet_Banking_System.Entites;

namespace InternetBanking.Service.Abstract
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(string userId, string accountNumber, decimal? initialDeposit = null, CancellationToken ct = default);
        Task<Account> GetByIdAsync(int accountId, CancellationToken ct = default);
        Task<Account> GetByNumberAsync(string accountNumber, CancellationToken ct = default);
        Task<List<Account>> GetUserAccountsAsync(string userId, CancellationToken ct = default);

        Task FreezeAsync(int accountId, CancellationToken ct = default);
        Task UnfreezeAsync(int accountId, CancellationToken ct = default);
        Task CloseAsync(int accountId, CancellationToken ct = default); //  Balance must be zero
    }
}
