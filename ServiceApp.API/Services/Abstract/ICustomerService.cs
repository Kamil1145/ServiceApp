using ServiceApp.Models;
using ServiceApp.Models.DTO;
using ServiceApp.Models.Entities;

namespace ServiceApp.API.Services.Abstract
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomer(Guid id);
        Task<IEnumerable<CustomerDto>> GetAllCustomers();
        Task<Customer> CreateNewCustomer(CustomerDto newUserDto);
    }
}
