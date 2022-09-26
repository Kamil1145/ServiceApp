using ServiceApp.ApiClient;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

namespace ServiceApp.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IApiClient _applicationClient;

        public RoleService(IApiClient applicationClient)
        {
            _applicationClient = applicationClient;
        }
        public async Task<IEnumerable<Role>> GetRoles()
        {
            return await _applicationClient.GetRolesAsync();
        }
    }
}