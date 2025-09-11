using Internet_Banking_System.Entites;

namespace InternetBanking.Service.Abstract
{
    public interface IAuthService
    {
        Task<User> LoginAsync(string username, string password, CancellationToken ct = default);
    }
}

