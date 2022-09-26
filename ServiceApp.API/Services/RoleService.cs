using AutoMapper;
using ServiceApp.API.DAL.Interfaces;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Services;

class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public IEnumerable<Role> GetRoles()
    {
        return _unitOfWork.Roles.GetAllRoles();
    }
}
