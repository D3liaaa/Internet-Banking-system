using Internet_Banking_System.Entites;

namespace InternetBanking.Service.Abstract
{
    public interface ITransactionService
    {
        Task<Transaction> DepositAsync(int accountId, decimal amount, string? description = null, CancellationToken ct = default);
        Task<Transaction> WithdrawAsync(int accountId, decimal amount, string? description = null, CancellationToken ct = default);
        Task<Transaction> TransferAsync(int fromAccountId, int toAccountId, decimal amount, string? description = null, CancellationToken ct = default);
        Task<List<Transaction>> GetStatementAsync(int accountId, DateTime from, DateTime to, CancellationToken ct = default);
    }
}
