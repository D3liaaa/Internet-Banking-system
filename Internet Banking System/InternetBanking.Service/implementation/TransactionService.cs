using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Service.Abstract;

namespace InternetBanking.Service.implementation
{
    public class TransactionService : ITransactionService
    {
        #region Properties
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        #endregion   

        #region Constructors
        public TransactionService(ITransactionRepository transactionRepository,IAccountRepository accountRepository )
        {
            _transactionRepository = transactionRepository;
                _accountRepository = accountRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<Transaction> DepositAsync(int accountId, decimal amount, string? description = null, CancellationToken ct = default)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Deposit amount must be greater than zero.");
            var account = await _accountRepository.GetByIdAsync(accountId, ct);
            if (account == null || !account.IsActive || account.IsFrozen)
                throw new InvalidOperationException("Account is not valid for deposit.");
            account.Balance += amount;

            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Type = TransactionType.Deposit,
                Date = DateTime.UtcNow,
                Description = description   

            };
            await _transactionRepository.AddAsync(transaction, ct);
            await _accountRepository.UpdateAsync(account, ct);

            return transaction;
        }

        public async Task<List<Transaction>> GetStatementAsync(int accountId, DateTime from, DateTime to, CancellationToken ct = default)
        {
            return await _transactionRepository.GetStatementAsync(accountId, from, to, ct);
        }

        public async Task<Transaction> TransferAsync(int fromAccountId, int toAccountId, decimal amount, string? description = null, CancellationToken ct = default)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Transfer amount must be greater than zero.");

            var fromAccount = await _accountRepository.GetByIdAsync(fromAccountId, ct);
            var toAccount = await _accountRepository.GetByIdAsync(toAccountId, ct);

            if (fromAccount == null || toAccount == null)
                throw new InvalidOperationException("Invalid accounts.");
            if (!fromAccount.IsActive || fromAccount.IsFrozen)
                throw new InvalidOperationException("Source account is not valid for transfer.");
            if (!toAccount.IsActive || toAccount.IsFrozen)
                throw new InvalidOperationException("Destination account is not valid for transfer.");
            if (fromAccount.Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
            var transaction = new Transaction
            {
                AccountId = fromAccountId,
                TargetAccountId = toAccountId,
                Amount = amount,
                Type = TransactionType.Transfer,
                Date = DateTime.UtcNow,
                Description = description
            };
            await _transactionRepository.AddAsync(transaction, ct);
            await _accountRepository.UpdateAsync(fromAccount, ct);
            await _accountRepository.UpdateAsync(toAccount, ct);
            return  transaction;
        }

        public async Task<Transaction> WithdrawAsync(int accountId, decimal amount, string? description = null, CancellationToken ct = default)
        {
            if (amount <= 0)
                throw new InvalidOperationException("Withdrawal amount must be greater than zero.");
            var account = await _accountRepository.GetByIdAsync(accountId, ct);
            if (account == null || !account.IsActive || account.IsFrozen)
                throw new InvalidOperationException("Account is not valid for withdrawal.");
            if (account.Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");
            account.Balance -= amount;
            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Type = TransactionType.Withdrawal,
                Date = DateTime.UtcNow,
                Description = description
            };
            await _transactionRepository.AddAsync(transaction, ct);
            await _accountRepository.UpdateAsync(account, ct);
            return transaction;
        }
        #endregion
    }
}
