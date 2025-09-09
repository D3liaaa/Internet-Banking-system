using Data;
using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Infrastructure.InfrastructureBase;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Repositories
{
    public class AccountRepository :GenericRepository<Account>, IAccountRepository
    {
        #region Fields
        private readonly DbSet<Account> _accountSet;
        #endregion

        #region Constructors
        public AccountRepository(InternetBankingContext appContext) : base(appContext)
        {
            _accountSet = appContext.Set<Account>();
        }

        #endregion

        #region Handle Functions

        public async Task<List<Account>> GetAllAsync()
        {
            return await _accountSet.Include(a => a.Transactions).AsNoTracking().ToListAsync();

            // return await _context.Set<Product>().ToListAsync();
            // return await _context.Product.AsQueryable().ToListAsync();
        }

        public async Task<Account?> GetByAccountNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            return await _accountSet
                           .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, ct);
        }

        public async Task<Account?> GetByIdWithTransactionsAsync(int id)
        {
            return await _accountSet
                         .Include(a => a.Transactions)
                         .AsNoTracking()
                         .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Account>> GetByUserIdAsync(string userId, CancellationToken ct = default)
        {
            return await _accountSet
                            .Where(a => a.UserId == userId)
                            .ToListAsync(ct);
        }

        #endregion

    }
}


