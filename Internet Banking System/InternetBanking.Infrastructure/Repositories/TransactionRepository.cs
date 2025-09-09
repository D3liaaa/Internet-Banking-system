using Data;
using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Infrastructure.InfrastructureBase;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Repositories
{
    public class TransactionRepository : GenericRepository<Transaction>, ITransactionRepository
    {
        #region Property
        private readonly DbSet<Transaction> _transactionSet;
        #endregion

        #region Constructors
        public TransactionRepository(InternetBankingContext context) : base(context)
        {
            _transactionSet = context.Set<Transaction>();
        }
        #endregion

        #region HandleFunctions
        public async Task<List<Transaction>> GetByAccountIdAsync(int accountId)
        {
            return await _transactionSet
                         .Where(t => t.AccountId == accountId || t.TargetAccountId == accountId)
                         .AsNoTracking()
                         .ToListAsync();
        } 
        #endregion
    }
}
