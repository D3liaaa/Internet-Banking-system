using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Service.Abstract;

namespace InternetBanking.Service.implementation
{
    public class AccountService:IAccountService
    {
        #region Property
        private readonly IAccountRepository _accountRepository;
        #endregion

        #region Constructors
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }
        #endregion

        #region Handle Functions
        public async Task CloseAsync(int accountId, CancellationToken ct = default)
        {
            var account = await _accountRepository.GetByIdAsync(accountId, ct);
            if (account.Balance != 0)
                throw new InvalidOperationException("Account balance must be zero before closing.");
            account.IsActive = false;
            await _accountRepository.UpdateAsync(account, ct);

        }

        public async Task<Account> CreateAccountAsync(string userId, string accountNumber, decimal? initialDeposit = null, CancellationToken ct = default)
        {
              var existing = await _accountRepository.GetByAccountNumberAsync(accountNumber, ct);
              if (existing != null)
                throw new InvalidOperationException("Account number already exists.");
            var account = new Account
            {
                UserId = userId,
                AccountNumber = accountNumber,
                Balance = initialDeposit ?? 0,
                CreatedDate = DateTime.UtcNow,
                Type = AccountType.Savings,
                IsActive = true,
                IsFrozen = false
            };
            await _accountRepository.AddAsync(account, ct);
            return account;


        }

        public async Task FreezeAsync(int accountId, CancellationToken ct = default)
        {
            var account = await _accountRepository.GetByIdAsync(accountId, ct);
            if(account.IsActive==false)
                throw new InvalidOperationException("Cannot freeze a closed account.");
            account.IsFrozen = true;
            await _accountRepository.UpdateAsync(account, ct);

        }

        public async Task<Account> GetByIdAsync(int accountId, CancellationToken ct = default)
        {
            var account = await _accountRepository.GetByIdAsync(accountId, ct);
            if (account == null)
                throw new KeyNotFoundException("Account not found.");
            return account;
        }

        public async Task<Account> GetByNumberAsync(string accountNumber, CancellationToken ct = default)
        {
            var account = await _accountRepository.GetByAccountNumberAsync(accountNumber, ct);
            if (account == null)
                throw new KeyNotFoundException("Account not found.");
            return account;
        }

        public async Task<List<Account>> GetUserAccountsAsync(string userId, CancellationToken ct = default)
        {
            return await _accountRepository.GetByUserIdAsync(userId, ct);
        }
        public async Task UnfreezeAsync(int accountId, CancellationToken ct = default)
        {
            var account = await _accountRepository.GetByIdAsync(accountId, ct);
            if (account == null)
                throw new KeyNotFoundException("Account not found.");
            if(account.IsActive==false)
                throw new InvalidOperationException("Cannot unfreeze a closed account.");
            account.IsFrozen = false;
             await _accountRepository.UpdateAsync(account, ct);
        } 
        #endregion
    }
}
