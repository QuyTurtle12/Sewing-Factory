

using SewingFactory.Models;

namespace SewingFactory.Services.Interface
{
    public interface IRoleService
    {
        Task<string?> GetRoleName(Guid roleID);
    }
}
