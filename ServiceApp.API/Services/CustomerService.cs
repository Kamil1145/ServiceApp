using System.Linq.Expressions;
using AutoMapper;
using ServiceApp.API.DAL.Interfaces;
using ServiceApp.API.Services.Abstract;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public CustomerService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IEmailService emailService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Customer> CreateNewCustomer(CustomerDto customerDto)
        {
            var newCustomer = _mapper.Map<CustomerDto, Customer>(customerDto);
            newCustomer.Id = Guid.NewGuid();
            newCustomer.CustomerId = Guid.NewGuid();
            newCustomer.CreatedAt = DateTime.Now;

            var role = _unitOfWork.Roles.GetAllRoles(a => a.RoleName == "Customer").FirstOrDefault();
            newCustomer.Roles.Add(role);
            _unitOfWork.Users.Insert(newCustomer);
            _unitOfWork.Save();

            _emailService.SendEmail("New account created",
            $"hello {newCustomer.FirstName} {newCustomer.LastName}" +
            $" <address>https://localhost:44334/activateaccount/{newCustomer.Id}</address>",
            newCustomer.Email,
            $"{newCustomer.FirstName} {newCustomer.LastName}");

            return newCustomer;
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
        {
            var users = _unitOfWork.Users.GetAllUsers(
                includes: new Expression<Func<User, object>>[] { a => a.Roles },
                filter: a => a.Roles.Any(a => a.RoleName == "Customer"));

            return users.Select(a => _mapper.Map<User, CustomerDto>(a));
        }

        public async Task<Customer> GetCustomer(Guid id)
        {
            return _unitOfWork.Users.GetUser(x => x.Id == id) as Customer;
        }
    }
}
