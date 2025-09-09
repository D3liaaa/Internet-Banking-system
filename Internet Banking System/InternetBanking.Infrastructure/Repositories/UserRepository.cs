using Data;
using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Infrastructure.InfrastructureBase;
using Microsoft.EntityFrameworkCore;

namespace InternetBanking.Infrastructure.Repositories
{
    public class UserRepository: GenericRepository<User>, IUserRepository
    {
        #region Properties
        private readonly DbSet<User> _userSet;
        private readonly InternetBankingContext _context;
        #endregion

        #region constructors
        public UserRepository(InternetBankingContext context): base(context)
        {
            _userSet = context.Set<User>();
        }
        #endregion

        #region HandleFunctions
        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _userSet
                         .Include(u => u.Accounts) 
                         .AsNoTracking()
                         .FirstOrDefaultAsync(u => u.UserName == username);
        } 
        #endregion
    }
}
