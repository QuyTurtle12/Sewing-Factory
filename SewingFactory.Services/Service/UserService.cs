using Microsoft.EntityFrameworkCore;
using SewingFactory.Models;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;


namespace SewingFactory.Services.Service
{
    public class UserService : IUserService
    {
        private readonly DatabaseContext _dbContext;

        public UserService(DatabaseContext dbContext)
        {
            this._dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<User> GetUser(Guid userID)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.ID == userID);
            return user;
        }

        public async Task<string?> GetUserName(Guid userID)
        {
            var user = await GetUser(userID);
            return user.Username;
        }

        public async Task<bool> IsValidUser(Guid userID)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.ID == userID) is not null;
        }
    }
}
