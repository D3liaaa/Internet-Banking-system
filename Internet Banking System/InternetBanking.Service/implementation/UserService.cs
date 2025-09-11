using static BCrypt.Net.BCrypt;
using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Service.Abstract;

namespace InternetBanking.Service.Implementation
{
    public class UserService : IUserService
    {
        #region Properties
        private readonly IUserRepository _userRepository;

        #endregion

        #region Constructors
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion

        #region Handle functions
        public async Task<bool> DeleteUserAsync(int userId, CancellationToken ct = default)
        {
            var existingUser = await _userRepository.GetByIdAsync(userId, ct);
            if (existingUser == null)
                throw new InvalidOperationException("User not found.");
            await _userRepository.DeleteAsync(existingUser, ct);
            return true;
        }

        public async Task<List<User>> GetAllUsersAsync(CancellationToken ct = default)
        {
            var users = await _userRepository.GetAllAsync(ct);
            return users.ToList();
        }

        public async Task<User?> GetUserByIdAsync(int userId, CancellationToken ct = default)
        {
            return await _userRepository.GetByIdAsync(userId, ct);
        }

        public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken ct = default)
        {
            return await _userRepository.GetByUsernameAsync(username, ct);
        }

        public async Task<User> RegisterUserAsync(User user, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new InvalidOperationException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new InvalidOperationException("Password cannot be empty.");

            // Check if username already exists
            var existingUser = await _userRepository.GetByUsernameAsync(user.UserName, ct);
            if (existingUser != null)
                throw new InvalidOperationException("Username already exists.");

            // Hash the password before saving
            user.PasswordHash = HashPassword(user.PasswordHash);
            await _userRepository.AddAsync(user, ct);
            return user;
        }

        public async Task<User> UpdateUserAsync(User user, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(user.UserName))
                throw new InvalidOperationException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(user.PasswordHash))
                throw new InvalidOperationException("Password cannot be empty.");

            // تأكد أن المستخدم موجود أصلًا
            var userInDb = await _userRepository.GetByIdAsync(user.Id, ct);
            if (userInDb == null)
                throw new InvalidOperationException("User not found.");

            // لو في مستخدم تاني بنفس الاسم
            var duplicateUser = await _userRepository.GetByUsernameAsync(user.UserName, ct);
            if (duplicateUser != null && duplicateUser.Id != user.Id)
                throw new InvalidOperationException("Username already exists.");

            if (!Verify(user.PasswordHash, userInDb.PasswordHash))
                user.PasswordHash = HashPassword(user.PasswordHash);
            else
                user.PasswordHash = userInDb.PasswordHash;


            await _userRepository.UpdateAsync(user, ct);
            return user;
        }


        #endregion
    }
}
