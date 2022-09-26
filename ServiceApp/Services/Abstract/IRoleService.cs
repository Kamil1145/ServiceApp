using ServiceApp.Models.Entities;

public interface IRoleService
{
    Task<IEnumerable<Role>> GetRoles();

}


