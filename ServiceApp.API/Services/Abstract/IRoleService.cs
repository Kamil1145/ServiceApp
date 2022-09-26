using ServiceApp.Models.Entities;

namespace ServiceApp.API.Services.Abstract
{
    public interface IRoleService
    {
        IEnumerable<Role> GetRoles();

    }
}
