using GravyFoodsApi.Data;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace GravyFoodsApi.MasjidServices
{
    public class CustomerInfoService : IRepository<CustomerInfo>, ICustomerInfoService
    {
        private readonly MasjidDBContext _context;

        public CustomerInfoService(MasjidDBContext context)
        {
            _context = context;
        }

        public async Task<ServiceResultWrapper<CustomerInfo>> Create(CustomerInfoDTO customerInfo)
        {
            // Implementation to add customer info to database
            // Check if email or phone number already exists
            bool isExisted = await CheckCustomerByNameAndEmail(customerInfo.PhoneNo, customerInfo.Email);

            if (isExisted)
            {
                //return null;
                return ServiceResultWrapper<CustomerInfo>.Fail($"Customer with '{customerInfo.Email}' OR '{customerInfo.PhoneNo}' already exists.");
            }

            CustomerInfo newCustomer = new CustomerInfo
            {

                CustomerId = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 10).ToUpper(),
                CustomerName = customerInfo.CustomerName,
                Address = customerInfo.Address,
                PhoneNo = customerInfo.PhoneNo,
                Email = customerInfo.Email

            };

            await _context.CustomerInfo.AddAsync(newCustomer);
            await _context.SaveChangesAsync();
            return ServiceResultWrapper<CustomerInfo>.Ok(newCustomer);
        }

        public Task<bool> CheckCustomerByNameAndEmail(string PhoneNo, string email)
        {
            var isExisted = _context.CustomerInfo.Any(c => c.Email == email || c.PhoneNo == PhoneNo);
            return Task.FromResult(isExisted);
        }

        public Task<IEnumerable<CustomerInfo>?> GetAllCustomersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerInfo?> GetCustomerInfoById(int Id)
        {
            var customer = _context.CustomerInfo.FirstOrDefault(c => c.Id == Id);
            return Task.FromResult(customer);
        }

        public Task<bool> UpdateCustomerInfoAsync(CustomerInfo customerInfo)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<CustomerInfo>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<CustomerInfo?> GetByIdAsync(object id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CustomerInfo entity)
        {
            throw new NotImplementedException();
        }

        public Task<CustomerInfo> AddAsync(CustomerInfo entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(object id)
        {
            throw new NotImplementedException();
        }
    }
}
