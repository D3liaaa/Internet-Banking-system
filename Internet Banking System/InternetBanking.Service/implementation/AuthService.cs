using static BCrypt.Net.BCrypt;
using Internet_Banking_System.Entites;
using InternetBanking.Infrastructure.Abstract;
using InternetBanking.Service.Abstract;

namespace InternetBanking.Service.Implementation
{
    public class AuthService : IAuthService
    {

        #region Properties
        private readonly IUserRepository _userRepository;
        #endregion

        #region Constructors
        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        #endregion


        #region Handle Functions
        public async Task<User> LoginAsync(string username, string password, CancellationToken ct = default)
        {
            // 1. check if user exist
            var user = await _userRepository.GetByUsernameAsync(username, ct);
            if (user == null)
                throw new InvalidOperationException("Invalid username or password.");

            // 2.check password
            bool isPasswordValid = Verify(password, user.PasswordHash);
            if (!isPasswordValid)
                throw new InvalidOperationException("Invalid username or password.");

            return user;
        } 
        #endregion
    }
}
