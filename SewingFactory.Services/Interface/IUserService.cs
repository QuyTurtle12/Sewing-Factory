using SewingFactory.Models;

namespace SewingFactory.Services.Interface
{
    public interface IUserService
    {
        Task<bool> IsValidUser(Guid userID);
        Task<string?> GetUserName(Guid userID);
        Task<User> GetUser(Guid userID);
    }
}
