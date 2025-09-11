using Data;
using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Service.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace InternetBanking.Service.Implementation
{
    public class TransactionService : ITransactionService
    {
        #region Properties
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly InternetBankingContext _DbContext; 
        #endregion   

        #region Constructors
        public TransactionService(ITransactionRepository transactionRepository,IAccountRepository accountRepository,InternetBankingContext internetBankingContext )
        {
            _transactionRepository = transactionRepository;
                _accountRepository = accountRepository;
            _DbContext = internetBankingContext;
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
            using var dbTransaction = await _DbContext.Database.BeginTransactionAsync(ct);
            try
            {
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
                var transactionOut = new Transaction
                {
                    AccountId = fromAccountId,
                    TargetAccountId = toAccountId,
                    Amount = amount,
                    Type = TransactionType.Transfer,
                    Date = DateTime.UtcNow,
                    Description = description
                };
                var transactionIn = new Transaction
                {
                    AccountId = toAccountId,
                    TargetAccountId = fromAccountId,
                    Amount = amount,
                    Type = TransactionType.Deposit,
                    Date = DateTime.UtcNow,
                    Description = "Transfer received"
                };
                await _transactionRepository.AddAsync(transactionOut, ct);
                await _transactionRepository.AddAsync(transactionIn, ct);
                await _accountRepository.UpdateAsync(fromAccount, ct);
                await _accountRepository.UpdateAsync(toAccount, ct);

                await _DbContext.SaveChangesAsync(ct);
                await dbTransaction.CommitAsync(ct);

                return transactionOut;

            }
            catch
            {
                // لو حصل مشكلة نعمل Rollback
                await dbTransaction.RollbackAsync(ct);
                throw;
            }
            
            
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
