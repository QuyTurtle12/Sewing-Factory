using Microsoft.EntityFrameworkCore;
using SewingFactory.Repositories.DBContext;
using SewingFactory.Services.Interface;

namespace SewingFactory.Services.Service
{
    public class RoleService : IRoleService
    {
        private readonly DatabaseContext _dbContext;

        public RoleService(DatabaseContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<string?> GetRoleName(Guid roleID)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.ID == roleID);
            return role.Name;
        }
    }
}