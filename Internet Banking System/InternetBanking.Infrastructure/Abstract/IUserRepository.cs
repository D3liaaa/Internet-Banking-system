
using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.InfrastructureBase;

namespace InternetBanking.Infrastructure.Abstract
{
    public interface IUserRepository : IGenericRepository<User>
    {
        public Task<User?> GetByUsernameAsync(string username);
        //public Task<User?> GetByEmailAsync(string email);
        //public Task<User?> GetByPhoneNumberAsync(string phoneNumber);
        //public Task<User?> GetByIdWithAccountsAsync(int id);
        //public Task<List<User>> GetAllWithAccountsAsync();
    }
}
