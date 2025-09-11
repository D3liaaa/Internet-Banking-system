using Internet_Banking_System.Entites;

namespace InternetBanking.Service.Abstract
{
    public interface IUserService
    {
        public Task<User> RegisterUserAsync(User user, CancellationToken ct = default);
        public Task<User?> GetUserByIdAsync(int userId, CancellationToken ct = default);
        public Task<User?> GetUserByUsernameAsync(string username, CancellationToken ct = default);
        public Task<List<User>> GetAllUsersAsync(CancellationToken ct = default);
        public Task<User> UpdateUserAsync(User user, CancellationToken ct = default);
        public Task<bool> DeleteUserAsync(int userId, CancellationToken ct = default);
    }
}
