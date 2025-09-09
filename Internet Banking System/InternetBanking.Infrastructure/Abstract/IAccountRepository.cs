using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.InfrastructureBase;


namespace InternetBanking.Infrastructure.Abstract
{
    public interface IAccountRepository:IGenericRepository<Account>
    {
        public Task<Account?> GetByIdWithTransactionsAsync(int id);

        public Task<List<Account>> GetAllAsync();
       
    }
}
